import axios from "axios";
import { apiClient } from "./apiClient";

export function setupInterceptors() {
  apiClient.interceptors.request.use((config) => {
    const auth = localStorage.getItem("auth");

    if (auth) {
      try {
        const parsed = JSON.parse(auth);
        config.headers.Authorization = `Bearer ${parsed.token}`;
      } catch {
        // ignore invalid auth
      }
    }

    return config;
  });

  apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (axios.isAxiosError(error)) {
        console.error("AXIOS ERROR:", {
          status: error.response?.status,
          data: error.response?.data,
          message: error.message,
        });

        // 🔥 EZ A LÉNYEG:
        return Promise.reject(error);
      }

      console.error("UNKNOWN ERROR:", error);

      return Promise.reject(error);
    },
  );
}
