import { apiClient } from "../client/apiClient";

export async function get<T>(
  url: string,
  params?: Record<string, string | number | boolean | undefined>,
): Promise<T> {
  const res = await apiClient.get<T>(url, { params });
  return res.data;
}

export async function post<TReq, TRes>(
  url: string,
  body?: TReq,
): Promise<TRes> {
  const res = await apiClient.post<TRes>(url, body);
  return res.data;
}

export async function del(url: string): Promise<void> {
  await apiClient.delete(url);
}
