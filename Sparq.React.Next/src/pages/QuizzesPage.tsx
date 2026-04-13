import { getQuizzes } from "@/api/client/quizzes-client";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";
import { useEffect, useState } from "react";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { QuizContainer } from "@/components/containers/QuizContainer";

export function QuizzesPage() {
  const [quizzes, setQuizzes] = useState<QuizResponseDto[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadContent() {
      setError(null);
      setIsLoading(true);
      try {
        const loadedQuizzes = await getQuizzes();
        setQuizzes(loadedQuizzes);
      } catch (e) {
        setError(e instanceof Error ? e.message : "Unknown error.");
      } finally {
        setIsLoading(false);
      }
    }
    loadContent();
  }, []);

  if (isLoading) {
    return <LoadingIndicator />;
  }

  return (
    <div className="mb-4 py-4">
      <h2 className="h2-style">Quizzes</h2>
      <QuizContainer quizzes={quizzes} error={error} />
    </div>
  );
}
