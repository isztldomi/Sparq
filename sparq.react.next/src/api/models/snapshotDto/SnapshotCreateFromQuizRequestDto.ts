import type { QuestionCreateRequestDto } from "@/api/models/questionDto/QuestionCreateRequestDto";

export interface SnapshotCreateFromQuizRequestDto {
  title: string;
  description: string;
  TimeLimit: number;
  questions: QuestionCreateRequestDto[];
}
