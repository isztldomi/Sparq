import { getQuizzes } from "@/api/client/quizzes-client";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";
import { useEffect, useState } from "react";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { MyQuizContainer } from "@/components/containers/MyQuizContainer";
import { QuizCreateButton } from "@/components/buttons/QuizCreateButton";

export function MyQuizzesPage() {
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
      <div className="grid grid-cols-1 sm:grid-cols-2">
        <h2 className="h2-style">My Quizzes</h2>
        <div className="text-right flex items-center justify-end pb-4">
          <QuizCreateButton />
        </div>
      </div>
      <MyQuizContainer quizzes={quizzes} error={error} />
    </div>
  );
}
