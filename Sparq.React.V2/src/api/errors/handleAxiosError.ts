type NormalizedError = {
  status: number;
  message: string;
  errors?: Record<string, string[]>;
};

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === "object" && value !== null;
}

export function normalizeError(e: unknown): NormalizedError {
  if (!isRecord(e)) {
    return {
      status: 0,
      message: "Unknown error",
    };
  }

  const status = typeof e.status === "number" ? e.status : 0;

  const message = typeof e.message === "string" ? e.message : "Unknown error";

  const errors = isRecord(e.errors)
    ? (e.errors as Record<string, string[]>)
    : undefined;

  return {
    status,
    message,
    errors,
  };
}
