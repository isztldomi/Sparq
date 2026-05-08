import type { SnapshotMetaDetailsResponseDto } from "../snapshot/snapshotTypes";

export type MyQuizSessionsListDto = {
  id: string;
  snapshotId: string;
  snapshot: SnapshotMetaDetailsResponseDto;
  startedAt: Date;
  endedAt: Date;
  currentQuestionId: string | null;
  isWaiting: boolean;
  isRunning: boolean;
};

export type CreatedSessionResponseDto = {
  id: string;
  snapshotId: string;
  createdAt: Date;
  pinCode: string;
  isWaiting: boolean;
  isRunning: boolean;
};

export type CreateSessionRequestDto = {
  quizId: string;
};
