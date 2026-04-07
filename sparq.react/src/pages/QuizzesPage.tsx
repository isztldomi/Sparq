import { useEffect, useState } from "react";
import { QuizContainer } from "@/components/containers/QuizContainer";
import { LoadingIndicator } from "@/components/LoadingIndicator";

type Quiz = {
  id: string;
  title: string;
  description: string;
};

export function QuizzesPage() {
  const [isLoading, setIsLoading] = useState(true);
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);

  useEffect(() => {
    const timer = setTimeout(() => {
      setQuizzes([
        {
          id: "1",
          title: "General Knowledge",
          description: "Basic general knowledge quiz.",
        },
        {
          id: "2",
          title: "React Basics",
          description: "Test your React fundamentals.",
        },
        {
          id: "3",
          title: "TypeScript Advanced",
          description: "Challenging TS concepts.",
        },
      ]);

      setIsLoading(false);
    }, 500);

    return () => clearTimeout(timer);
  }, []);

  if (isLoading) {
    return <LoadingIndicator />;
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
      <h1 style={{ color: "var(--color-text-primary)" }}>Quizzes</h1>

      <p style={{ color: "var(--color-text-secondary)" }}>
        Choose a quiz to start.
      </p>

      <QuizContainer quizzes={quizzes} />
    </div>
  );
}
