import type {
  QuestionResponseDto,
  QuestionCreateRequestDto,
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
