import type {
  SnapshotCreateFromQuizRequestDto,
  SnapshotResponseDto,
  SnapshotUI,
} from "@/features/snapshot/snapshotTypes";

export interface QuizResponseDto {
  id: number;
  isPublic: boolean;
  lastSnapshot: SnapshotResponseDto;
  snapshots: SnapshotResponseDto[];
}

export interface QuizCreateRequestDto {
  isPublic: boolean;
  snapshots: SnapshotCreateFromQuizRequestDto[];
}

export type QuizUI = Omit<QuizCreateRequestDto, "snapshots"> & {
  snapshots: SnapshotUI[];
};
