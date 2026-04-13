import type { QuestionResponseDto } from "@/api/models/questionDto/QuizWithLastSnapshotResponseDto";

export interface SnapshotResponseDto {
  id: number;
  title: string;
  description: string;
  timeLimit: number;
  questions: QuestionResponseDto[];
}
