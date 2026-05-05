import type {
  QuestionResponseDto,
  QuestionCreateRequestDto,
  QuestionUI,
} from "@/features/question/questionTypes";

export interface SnapshotResponseDto {
  id: number;
  title: string;
  description: string;
  timeLimit: number;
  questions: QuestionResponseDto[];
}

export interface SnapshotCreateFromQuizRequestDto {
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  questions: QuestionCreateRequestDto[];
}

export type SnapshotUI = Omit<SnapshotCreateFromQuizRequestDto, "questions"> & {
  questions: QuestionUI[];
};
