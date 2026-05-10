export function buildQuery(
  params?: Record<string, string | number | boolean | undefined>,
) {
  if (!params) return "";

  const query = Object.entries(params)
    .filter(
      ([, value]) => value !== undefined && value !== null && value !== "",
    )
    .map(
      ([key, value]) =>
        `${encodeURIComponent(key)}=${encodeURIComponent(value as string)}`,
    )
    .join("&");

  return query ? `?${query}` : "";
}
