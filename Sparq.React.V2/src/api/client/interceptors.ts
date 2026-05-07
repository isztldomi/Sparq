import { apiClient } from "./apiClient";
import { refreshTokenApi } from "@/api/services/authService";

let isRefreshing = false;
let queue: ((token: string) => void)[] = [];

export function setupInterceptors() {
  // =====================
  // REQUEST INTERCEPTOR
  // =====================
  apiClient.interceptors.request.use((config) => {
    const auth = localStorage.getItem("auth");

    if (auth) {
      try {
        const parsed = JSON.parse(auth);

        config.headers = config.headers ?? {};
        config.headers.Authorization = `Bearer ${parsed.token}`;
      } catch {
        console.warn("Invalid auth in localStorage");
      }
    }

    return config;
  });

  // =====================
  // RESPONSE INTERCEPTOR
  // =====================
  apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;
      // =====================
      // 401 HANDLING
      // =====================
      if (error.response?.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;

        try {
          const auth = JSON.parse(localStorage.getItem("auth") || "{}");

          if (isRefreshing) {
            return new Promise((resolve) => {
              queue.push((token: string) => {
                originalRequest.headers.Authorization = `Bearer ${token}`;
                resolve(apiClient(originalRequest));
              });
            });
          }

          isRefreshing = true;

          const res = await refreshTokenApi(auth.refreshToken);

          const newAuth = {
            token: res.authToken,
            refreshToken: res.refreshToken,
          };

          localStorage.setItem("auth", JSON.stringify(newAuth));

          isRefreshing = false;

          queue.forEach((cb) => cb(res.authToken));
          queue = [];

          originalRequest.headers.Authorization = `Bearer ${res.authToken}`;

          return apiClient(originalRequest);
        } catch (refreshError) {
          console.error("Refresh failed → logout user");

          localStorage.removeItem("auth");

          return Promise.reject(refreshError);
        }
      }

      return Promise.reject(error);
    },
  );
}
