import type { AnswerCreateRequestDto } from "@/api/models/answerDto/AnswerCreateRequestDto";

export interface QuestionCreateRequestDto {
  title: string;
  text: string;
  mediaUrl: string | null;
  TimeLimit: number;
  point: number;
  answers: AnswerCreateRequestDto[];
}
