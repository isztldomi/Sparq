import { post, getBlob } from "../http/http";
import type { MediaUploadResponseDto } from "@/features/media/mediaTypes";

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
