export interface AnswerResponseDto {
  id: string;
  text: string;
  isCorrect: boolean;
  order: number;
}

export interface AnswerCreateRequestDto {
  text: string;
  isCorrect: boolean;
  order: number;
}

export type AnswerUI = AnswerCreateRequestDto & {
  id: string;
};

export type CurrentQuestionAnswerWithoutResultDto = {
  id: string;
  text: string;
  order: number;
};

export type CurrentQuestionAnswerWithResultDto = {
  id: string;
  text: string;
  isCorrect: boolean;
  order: number;
};

export type SubmitAnswerRequestDto = {
  sessionId: string;
  questionId: string;
  answerId: string;
  extUserId: string | null;
};

export type SessionQuestionAnswersResponseDto = {
  sessionId: string;
  questionId: string;
  answers: ParticipantAnswerDto[];
};

export type ParticipantAnswerDto = {
  participantId: string;
  displayName: string;
  extUserId: null;
  userId: string;
  answerId: string;
  answerText: string;
  isCorrect: boolean;
  pointsEarned: number;
  answeredAt: string;
};
