import { HttpError } from "@/api/errors/HttpError";

export function normalizeError(e: unknown) {
  if (e instanceof HttpError) {
    return {
      status: e.status,
      message: e.message,
      errors: e.errors,
    };
  }

  return {
    status: 0,
    message: "Unknown error",
  };
}
