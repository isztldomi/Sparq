import type {
  QuestionResponseDto,
  QuestionCreateRequestDto,
  QuestionUI,
} from "@/features/question/questionTypes";

export type SnapshotResponseDto = {
  id: string;
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  questions: QuestionResponseDto[];
};

export type SnapshotCreateFromQuizRequestDto = {
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  questions: QuestionCreateRequestDto[];
};

export type SnapshotUI = Omit<SnapshotCreateFromQuizRequestDto, "questions"> & {
  questions: QuestionUI[];
};

export type SnapshotCreateRequestDto = {
  quizId: string;
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  questions: QuestionCreateRequestDto[];
};

export type SnapshotMetaDetailsResponseDto = {
  id: string;
  quizId: string;
  snapshotNumber: number;
  title: string;
  description: string;
  createdAt: Date;
};

export type SnapshotMetaDetails2ResponseDto = {
  title: string;
  description: string;
};
