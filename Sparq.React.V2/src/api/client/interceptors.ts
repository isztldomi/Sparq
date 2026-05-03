import axios from "axios";
import { apiClient } from "./apiClient";
import { HttpError } from "@/api/errors/HttpError";
import type { ProblemDetails } from "@/api/models/ProblemDetails";

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
      if (axios.isAxiosError<ProblemDetails>(error)) {
        throw HttpError.fromAxios(error);
      }

      throw new HttpError(0, "Network or unknown error");
    },
  );
}
