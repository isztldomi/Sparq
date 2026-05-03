import axios from "axios";
import { HttpError } from "@/api/errors/HttpError";
import type { ProblemDetails } from "@/api/models/ProblemDetails";
import { apiClient } from "./apiClient";

export function setupInterceptors() {
  apiClient.interceptors.request.use((config) => {
    const auth = localStorage.getItem("auth");

    if (auth) {
      const parsed = JSON.parse(auth);
      config.headers.Authorization = `Bearer ${parsed.token}`;
    }

    return config;
  });

  apiClient.interceptors.response.use(
    (res) => res,
    (error) => {
      if (axios.isAxiosError(error)) {
        const status = error.response?.status ?? 0;
        const data = error.response?.data as ProblemDetails | undefined;

        const message =
          data?.detail ?? data?.title ?? error.message ?? "Unknown error";

        throw new HttpError(status, message, data?.errors);
      }

      throw new HttpError(0, "Unknown error");
    },
  );
}
