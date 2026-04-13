import { MyQuizCard } from "@/components/cards/MyQuizCard";

import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";

export function MyQuizContainer({
  quizzes,
  error,
}: {
  quizzes: QuizResponseDto[];
  error: string | null;
}) {
  if (error) {
    return <div className="text-red-500">Error: {error}</div>;
  } else if (quizzes.length === 0) {
    return <div>No quizzes available.</div>;
  } else {
    return (
      <div className="grid grid-cols-1 gap-4">
        {quizzes.map((quiz) => MyQuizCard(quiz))}
      </div>
    );
  }
}
