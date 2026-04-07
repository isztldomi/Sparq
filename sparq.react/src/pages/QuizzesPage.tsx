import { useEffect, useState } from "react";
import { QuizContainer } from "@/components/containers/QuizContainer";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { getQuizzes } from "@/api/client/quizzes-client";
import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";

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
    <>
      {error ? <ErrorAlert message={error} /> : null}
      <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
        <h1 style={{ color: "var(--color-text-primary)" }}>Quizzes</h1>

        <p style={{ color: "var(--color-text-secondary)" }}>
          Choose a quiz to start.
        </p>

        <QuizContainer quizzes={quizzes} />
      </div>
    </>
  );
}
