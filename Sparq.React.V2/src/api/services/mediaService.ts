import { post } from "../http/http";
import type { MediaUploadResponseDto } from "@/features/media/mediaTypes";

export function uploadMediaApi(
  file: FormData,
): Promise<MediaUploadResponseDto> {
  return post("/media/upload", file, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}
