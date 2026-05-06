import type { QuestionCreateRequestDto } from "@/features/question/questionTypes";
import type { QuestionUI } from "@/features/question/questionTypes";
import { mapAnswerUIToDto } from "@/features/answer/answerMapper";

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
