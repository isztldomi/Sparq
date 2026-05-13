import type {
  SessionQuestionAnswersResponseDto,
  SubmitAnswerRequestDto,
} from "@/features/answer/answerTypes";
import { get, post } from "../http/http";
import { buildQuery } from "@/api/core/queryString";

export function submitAnswerApi(
  data: SubmitAnswerRequestDto,
): Promise<boolean> {
  return post(`/answer/submit`, data);
}

export function getSessionQuestionAnswersApi(
  sessionId: string,
  questionId: string,
  extUserId?: string,
): Promise<SessionQuestionAnswersResponseDto> {
  return get(
    `/answer/session/${sessionId}/question/${questionId}${buildQuery({ extUserId })}`,
  );
}
