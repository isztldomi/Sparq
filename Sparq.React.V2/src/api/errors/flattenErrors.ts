export function flattenErrors(
  errors?: Record<string, string[]>,
): { field: string; message: string }[] {
  if (!errors) return [];

  return Object.entries(errors).flatMap(([field, messages]) =>
    messages.map((message) => ({
      field,
      message,
    })),
  );
}
