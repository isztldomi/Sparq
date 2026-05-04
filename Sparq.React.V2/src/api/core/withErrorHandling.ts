import { toApiError } from "./toApiError";

export const withErrorHandling = async <T>(fn: () => Promise<T>) => {
  try {
    const data = await fn();
    return { data };
  } catch (e: unknown) {
    return { error: toApiError(e) };
  }
};
