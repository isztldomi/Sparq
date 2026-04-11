import { get, postAsJson } from "@/api/client/http";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";
import type { QuizCreateRequestDto } from "@/api/models/quizDto/QuizCreateRequestDto";

export async function getQuizzes(count?: number): Promise<QuizResponseDto[]> {
  return await get<QuizResponseDto[]>(
    "quiz",
    count ? { count: count.toString() } : undefined,
  );
}

export async function createQuiz(data: QuizCreateRequestDto) {
  return await postAsJson<QuizCreateRequestDto, QuizResponseDto>("quiz", data);
}
