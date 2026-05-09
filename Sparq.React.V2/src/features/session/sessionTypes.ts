import type {
  SnapshotMetaDetails2ResponseDto,
  SnapshotMetaDetailsResponseDto,
} from "../snapshot/snapshotTypes";

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

export type SessionPublicWaitingListDto = {
  id: string;
  snapshot: SnapshotMetaDetails2ResponseDto;
};

export type JoinSessionRequestDto = {
  sessionId: string;
  pinCode: string;
  nickname: string;
};
