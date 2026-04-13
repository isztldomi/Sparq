import type { QuizResponseDto } from "@/api/models/quizDto/QuizResponseDto";

export function QuizCard({ quiz }: { quiz: QuizResponseDto }) {
  return (
    <div className="card">
      <h2>{quiz.lastSnapshot.title}</h2>

      <p style={{ color: "var(--color-text-secondary)" }}>
        {quiz.lastSnapshot.description}
      </p>

      <div
        style={{
          display: "flex",
          justifyContent: "flex-end",
          marginTop: "1rem",
        }}
      >
        <button className="button-primary">Start</button>
      </div>
    </div>
  );
}
