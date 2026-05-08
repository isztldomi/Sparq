import type { SnapshotMetaDetailsResponseDto } from "../snapshot/snapshotTypes";

export type MyQuizSessionsListDto = {
  id: number;
  snapshotId: number;
  snapshot: SnapshotMetaDetailsResponseDto;
  startedAt: Date;
  endedAt: Date;
  currentQuestionId: number | null;
  isWaiting: boolean;
  isRunning: boolean;
};

export type CreatedSessionResponseDto = {
  id: number;
  snapshotId: number;
  createdAt: Date;
  pinCode: string;
  isWaiting: boolean;
  isRunning: boolean;
};

export type CreateSessionRequestDto = {
  quizId: number;
};
