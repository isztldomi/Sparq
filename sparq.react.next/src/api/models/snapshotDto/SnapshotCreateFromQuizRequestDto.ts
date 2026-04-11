import type { QuestionCreateRequestDto } from "@/api/models/questionDto/QuestionCreateRequestDto";

export interface SnapshotCreateFromQuizRequestDto {
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  questions: QuestionCreateRequestDto[];
}
