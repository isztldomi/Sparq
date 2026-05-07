import { post } from "../http/http";
import type {
  SnapshotCreateRequestDto,
  SnapshotResponseDto,
} from "@/features/snapshot/snapshotTypes";

export function createSnapshotApi(
  data: SnapshotCreateRequestDto,
): Promise<SnapshotResponseDto> {
  return post("/snapshot", data);
}
