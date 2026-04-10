import type { AnswerResponseDto } from "@/api/models/answerDto/AnswerResponseDto";

export interface QuestionResponseDto {
  id: number;
  title: string;
  text: string;
  mediaUrl: string;
  point: number;
  answers: AnswerResponseDto[];
}
