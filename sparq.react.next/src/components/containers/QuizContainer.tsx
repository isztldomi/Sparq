import { QuizCard } from "@/components/cards/QuizCard";

import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";

export function QuizContainer({
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
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {quizzes.map((quiz) => QuizCard({ lastSnapshot: quiz.lastSnapshot }))}
      </div>
    );
  }
}
