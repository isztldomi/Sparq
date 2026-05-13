import { get } from "../http/http";
import type {
  CurrentSessionQuestionStateWithoutResultDto,
  CurrentSessionQuestionStateWithResultDto,
} from "@/features/question/questionTypes";
import { buildQuery } from "../core/queryString";

export function getCurrentQuestionWithoutResultApi(
  sessionId: string,
  extUserId?: string,
): Promise<CurrentSessionQuestionStateWithoutResultDto> {
  return get(
    `/question/${sessionId}/without-result${buildQuery({ extUserId })}`,
  );
}

export function getCurrentQuestionWithResultApi(
  sessionId: string,
  extUserId?: string,
): Promise<CurrentSessionQuestionStateWithResultDto> {
  return get(`/question/${sessionId}/with-result${buildQuery({ extUserId })}`);
}
