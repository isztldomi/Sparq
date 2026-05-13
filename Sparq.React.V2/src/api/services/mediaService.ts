import { post, getBlob } from "../http/http";
import type { MediaUploadResponseDto } from "@/features/media/mediaTypes";
import { buildQuery } from "@/api/core/queryString";

export function uploadMediaApi(
  file: FormData,
): Promise<MediaUploadResponseDto> {
  return post("/media/upload", file, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}

export function getMediaBlobApi(id: string | number): Promise<Blob> {
  return getBlob(`/media/${id}`);
}

export function getMediaBlobSessionApi(
  sessionId: string,
  mediaId: string,
  extUserId?: string,
): Promise<Blob> {
  return getBlob(
    `/media/${mediaId}/session/${sessionId}${buildQuery({ extUserId })}`,
  );
}
