import { AxiosError } from "axios";
import type { ProblemDetails } from "@/api/models/ProblemDetails";

export const toApiError = (e: unknown) => {
  const error = e as AxiosError<ProblemDetails>;

  return {
    status: error.response?.status,
    data: error.response?.data,
  };
};
