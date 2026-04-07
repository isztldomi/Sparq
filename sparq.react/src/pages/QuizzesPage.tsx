import { useEffect, useState } from "react";

type Quiz = {
  id: string;
  title: string;
  description: string;
  difficulty: "easy" | "medium" | "hard";
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
          difficulty: "easy",
        },
        {
          id: "2",
          title: "React Basics",
          description: "Test your React fundamentals.",
          difficulty: "medium",
        },
        {
          id: "3",
          title: "TypeScript Advanced",
          description: "Challenging TS concepts.",
          difficulty: "hard",
        },
      ]);

      setIsLoading(false);
    }, 500);

    return () => clearTimeout(timer);
  }, []);

  if (isLoading) {
    return (
      <div
        style={{
          color: "var(--color-text-secondary)",
          textAlign: "center",
          marginTop: "2rem",
        }}
      >
        Loading quizzes...
      </div>
    );
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
      <h1 style={{ color: "var(--color-text-primary)" }}>Quizzes</h1>

      <p style={{ color: "var(--color-text-secondary)" }}>
        Choose a quiz to start.
      </p>

      <div style={{ display: "grid", gap: "1rem" }}>
        {quizzes.map((quiz) => (
          <div key={quiz.id} className="card">
            <h2>{quiz.title}</h2>

            <p style={{ color: "var(--color-text-secondary)" }}>
              {quiz.description}
            </p>

            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginTop: "1rem",
              }}
            >
              {/* Difficulty badge */}
              <span
                style={{
                  padding: "0.25rem 0.5rem",
                  borderRadius: "0.5rem",
                  border: "1px solid",
                  color:
                    quiz.difficulty === "easy"
                      ? "var(--color-success-text)"
                      : quiz.difficulty === "medium"
                        ? "var(--color-warning-text)"
                        : "var(--color-error-text)",
                  background:
                    quiz.difficulty === "easy"
                      ? "var(--color-success-bg)"
                      : quiz.difficulty === "medium"
                        ? "var(--color-warning-bg)"
                        : "var(--color-error-bg)",
                }}
              >
                {quiz.difficulty.toUpperCase()}
              </span>

              {/* CTA button */}
              <button className="button-primary">Start</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
