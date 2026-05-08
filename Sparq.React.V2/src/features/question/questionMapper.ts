import type {
  QuestionCreateRequestDto,
  QuestionResponseDto,
} from "@/features/question/questionTypes";
import type { QuestionUI } from "@/features/question/questionTypes";
import {
  mapAnswerUIToDto,
  mapAnswerResponseDtoToUI,
} from "@/features/answer/answerMapper";

export function mapQuestionUIToDto(q: QuestionUI): QuestionCreateRequestDto {
  return {
    title: q.title,
    text: q.text,
    mediaId: q.mediaId ?? null,
    timeLimit: q.timeLimit,
    point: q.point,
    answers: q.answers.map(mapAnswerUIToDto),
  };
}

export function mapQuestionDtoToUI(q: QuestionResponseDto): QuestionUI {
  return {
    id: q.id,
    isOpen: false,
    title: q.title,
    text: q.text,
    mediaId: q.mediaId ?? null,
    mediaFile: null,
    mediaPreviewUrl: null,
    timeLimit: q.timeLimit,
    point: q.point,
    answers: q.answers.map(mapAnswerResponseDtoToUI),
  };
}
