import type {
  SnapshotCreateFromQuizRequestDto,
  SnapshotResponseDto,
  SnapshotUI,
} from "@/features/snapshot/snapshotTypes";

export type QuizResponseDto = {
  id: string;
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
  id: string;
  isPublic: boolean;
  lastSnapshot: {
    id: string;
    title: string;
    description: string;
  };
};
