import type {
  SnapshotMetaDetails2ResponseDto,
  SnapshotMetaDetailsResponseDto,
} from "../snapshot/snapshotTypes";

export const SessionStatus = {
  Created: 0,
  Waiting: 1,
  Running: 2,
  Finished: 3,
} as const;

export type SessionStatus = (typeof SessionStatus)[keyof typeof SessionStatus];

export type MyQuizSessionsListDto = {
  id: string;
  snapshotId: string;
  snapshot: SnapshotMetaDetailsResponseDto;
  startedAt: string;
  endedAt: string;
  currentQuestionId: string | null;
  status: SessionStatus;
};

export type CreatedSessionResponseDto = {
  id: string;
  snapshotId: string;
  createdAt: string;
  pinCode: string;
  status: SessionStatus;
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

export type JoinSessionResponseDto = { externalUserId: string | null };

export type SessionStatusResponseDto = {
  status: SessionStatus;
};

export type quitSessionRequestDto = {
  sessionId: string;
  externalUserId: string | null;
};
