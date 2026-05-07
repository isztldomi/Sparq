import { clearAuth, getAuth, setAuth } from "@/features/auth/authStorage";
import { apiClient } from "./apiClient";
import { refreshTokenApi } from "@/api/services/authService";

let isRefreshing = false;
let queue: ((token: string) => void)[] = [];

export function setupInterceptors() {
  // =====================
  // REQUEST INTERCEPTOR
  // =====================
  apiClient.interceptors.request.use((config) => {
    const auth = getAuth();

    if (auth) {
      config.headers = config.headers ?? {};
      config.headers.Authorization = `Bearer ${auth.token}`;
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
      if (
        error.response?.status === 401 &&
        !originalRequest._retry &&
        !originalRequest.url?.includes("/users/refresh")
      ) {
        originalRequest._retry = true;

        const auth = getAuth();

        if (!auth?.refreshToken) {
          clearAuth();

          return Promise.reject(error);
        }

        try {
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

          setAuth(newAuth);

          queue.forEach((cb) => cb(res.authToken));

          queue = [];

          isRefreshing = false;

          originalRequest.headers.Authorization = `Bearer ${res.authToken}`;

          return apiClient(originalRequest);
        } catch (refreshError) {
          console.error("Refresh failed → logout user");

          clearAuth();

          queue = [];
          isRefreshing = false;

          return Promise.reject(refreshError);
        }
      }

      return Promise.reject(error);
    },
  );
}
