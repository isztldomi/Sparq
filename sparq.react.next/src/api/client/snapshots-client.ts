import { get } from "@/api/client/http";
import type { SnapshotResponseDto } from "../models/snapshotDto/SnapshotResponseDto";

export async function getSnapshotById(
  id: number,
): Promise<SnapshotResponseDto> {
  return await get<SnapshotResponseDto>(`snapshot/${id}`);
}
