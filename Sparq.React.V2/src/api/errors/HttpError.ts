import type { AxiosError } from "axios";

type RawApiError = {
  title?: string;
  status?: number;
  errors?: Record<string, string[]>;
  message?: string;
};

export class HttpError extends Error {
  public readonly status: number;
  public readonly errors?: Record<string, string[]>;

  constructor(
    status: number,
    message: string,
    errors?: Record<string, string[]>,
  ) {
    super(message);
    this.status = status;
    this.errors = errors;
  }

  static fromAxios(error: unknown): HttpError {
    const axiosError = error as AxiosError<RawApiError>;

    const data = axiosError.response?.data;

    const status = axiosError.response?.status ?? data?.status ?? 0;

    const message =
      data?.title ?? data?.message ?? axiosError.message ?? "Unknown error";

    return new HttpError(status, message, data?.errors);
  }
}
