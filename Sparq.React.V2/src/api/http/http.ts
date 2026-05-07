import type { AxiosRequestConfig, AxiosResponse } from "axios";
import { apiClient } from "../client/apiClient";

/**
 * CORE REQUEST ENGINE
 */
async function request<T>(config: AxiosRequestConfig): Promise<T> {
  const res: AxiosResponse<T> = await apiClient.request<T>(config);
  return res.data;
}

/**
 * GET
 */
export function get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
  return request<T>({
    ...config,
    method: "GET",
    url,
  });
}

/**
 * POST
 */
export function post<TReq, TRes>(
  url: string,
  body?: TReq,
  config?: AxiosRequestConfig,
): Promise<TRes> {
  return request<TRes>({
    ...config,
    method: "POST",
    url,
    data: body,
  });
}

/**
 * PATCH
 */
export function patch<TReq, TRes>(
  url: string,
  body?: TReq,
  config?: AxiosRequestConfig,
): Promise<TRes> {
  return request<TRes>({
    ...config,
    method: "PATCH",
    url,
    data: body,
  });
}

/**
 * DELETE
 */
export function del<T = void>(
  url: string,
  config?: AxiosRequestConfig,
): Promise<T> {
  return request<T>({
    ...config,
    method: "DELETE",
    url,
  });
}

/**
 * BLOB / FILE DOWNLOAD
 * (FONTOS: ezt külön kell kezelni)
 */
export function getBlob(
  url: string,
  config?: AxiosRequestConfig,
): Promise<Blob> {
  return request<Blob>({
    ...config,
    method: "GET",
    url,
    responseType: "blob",
  });
}
