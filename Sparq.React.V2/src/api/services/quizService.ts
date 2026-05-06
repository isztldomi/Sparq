import { post } from "../http/http";
import type { QuizCreateRequestDto } from "@/features/quiz/quizTypes";
import type { QuizResponseDto } from "@/features/quiz/quizTypes";

export function createQuizApi(
  data: QuizCreateRequestDto,
): Promise<QuizResponseDto> {
  return post("/quiz", data);
}
