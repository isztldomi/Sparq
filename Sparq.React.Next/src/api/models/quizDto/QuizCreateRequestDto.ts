import type { SnapshotCreateFromQuizRequestDto } from "@/api/models/snapshotDto/SnapshotCreateFromQuizRequestDto";

export interface QuizCreateRequestDto {
  isPublic: boolean;
  snapshots: SnapshotCreateFromQuizRequestDto[];
}
