export function flattenErrors(
  errors?: Record<string, string[]>,
): { field: string; message: string }[] {
  //console.log("flattenErrors input:", errors);

  if (!errors) return [];

  const result = Object.entries(errors).flatMap(([field, messages]) =>
    messages.map((message) => ({
      field,
      message,
    })),
  );

  //console.log("flattenErrors output:", result);

  return result;
}
