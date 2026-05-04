import type { AnswerCreateRequestDto } from "@/features/answer/answerTypes";
import type { AnswerUI } from "@/features/answer/answerTypes";

export function mapAnswerUIToDto(a: AnswerUI): AnswerCreateRequestDto {
  return {
    text: a.text,
    isCorrect: a.isCorrect,
  };
}
