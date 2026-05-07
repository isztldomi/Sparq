import type {
  SnapshotCreateFromQuizRequestDto,
  SnapshotResponseDto,
  SnapshotUI,
} from "@/features/snapshot/snapshotTypes";

export type QuizResponseDto = {
  id: number;
  isPublic: boolean;
  lastSnapshot: SnapshotResponseDto;
};

export type QuizCreateRequestDto = {
  isPublic: boolean;
  snapshots: SnapshotCreateFromQuizRequestDto[];
};

export type QuizUI = Omit<QuizCreateRequestDto, "snapshots"> & {
  snapshots: SnapshotUI[];
};

export type MyQuizListDto = {
  id: number;
  isPublic: boolean;
  lastSnapshot: {
    id: number;
    title: string;
    description: string;
  };
};
