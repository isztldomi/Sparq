import type { AnswerCreateRequestDto } from "@/api/models/answerDto/AnswerCreateRequestDto";

export interface QuestionCreateRequestDto {
  title: string;
  text: string;
  mediaUrl: string | null;
  timeLimit: number;
  point: number;
  answers: AnswerCreateRequestDto[];
}
