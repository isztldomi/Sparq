import type {
  QuestionResponseDto,
  QuestionCreateRequestDto,
  QuestionUI,
} from "@/features/question/questionTypes";

export type SnapshotResponseDto = {
  id: number;
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
  id: number;
  quizId: number;
  snapshotNumber: number;
  title: string;
  description: string;
  createdAt: Date;
};
