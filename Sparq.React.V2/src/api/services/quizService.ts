import type { PagedResult } from "@/features/page/pageTypes";
import { post, get, patch } from "../http/http";
import type {
  MyQuizListDto,
  QuizCreateRequestDto,
} from "@/features/quiz/quizTypes";
import type { QuizResponseDto } from "@/features/quiz/quizTypes";
import type { MyQuizSessionsListDto } from "@/features/session/sessionTypes";

export function createQuizApi(
  data: QuizCreateRequestDto,
): Promise<QuizResponseDto> {
  return post("/quiz", data);
}

export function getMyQuizzesApi(
  page: number,
  pageSize: number,
): Promise<PagedResult<MyQuizListDto>> {
  return get(`/quiz/mine?page=${page}&pageSize=${pageSize}`);
}

export function getQuizByIdApi(id: number): Promise<QuizResponseDto> {
  return get(`/quiz/${id}`);
}

export function deactivateQuizByIdApi(id: number): Promise<void> {
  return patch<void, void>(`/quiz/${id}/deactivate`);
}

export function getQuizSessionsByIdApi(
  id: number,
  page: number,
  pageSize: number,
): Promise<PagedResult<MyQuizSessionsListDto>> {
  return get(`/quiz/${id}/sessions?page=${page}&pageSize=${pageSize}`);
}
