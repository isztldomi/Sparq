import type { QuestionResponseDto } from "@/api/models/questionDto/QuestionResponseDto";

export interface SnapshotResponseDto {
  id: number;
  title: string;
  description: string;
  timeLimit: number;
  questions: QuestionResponseDto[];
}
