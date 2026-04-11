import type { SnapshotResponseDto } from "@/api/models/snapshotDto/SnapshotResponseDto";

export interface QuizResponseDto {
  id: number;
  isPublic: boolean;
  lastSnapshot: SnapshotResponseDto;
  snapshots: SnapshotResponseDto[];
}
