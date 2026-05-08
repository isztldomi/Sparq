import type {
  AnswerCreateRequestDto,
  AnswerResponseDto,
} from "@/features/answer/answerTypes";
import type { AnswerUI } from "@/features/answer/answerTypes";

export function mapAnswerUIToDto(a: AnswerUI): AnswerCreateRequestDto {
  return {
    text: a.text,
    isCorrect: a.isCorrect,
  };
}

export function mapAnswerResponseDtoToUI(a: AnswerResponseDto): AnswerUI {
  return {
    id: a.id,
    text: a.text,
    isCorrect: a.isCorrect,
  };
}
