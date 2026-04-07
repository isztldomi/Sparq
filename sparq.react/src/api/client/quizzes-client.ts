import { get } from "@/api/client/http";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";

export async function getQuizzes(count?: number): Promise<QuizResponseDto[]> {
  return await get<QuizResponseDto[]>(
    "quiz",
    count ? { count: count.toString() } : undefined,
  );
}
