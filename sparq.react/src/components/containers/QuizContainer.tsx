import { QuizCard } from "@/components/cards/QuizCard";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";

export function QuizContainer({ quizzes }: { quizzes: QuizResponseDto[] }) {
  return (
    <div style={{ display: "grid", gap: "1rem" }}>
      {quizzes.map((quiz) => (
        <QuizCard key={quiz.id} quiz={quiz} />
      ))}
    </div>
  );
}
