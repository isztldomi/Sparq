type Quiz = {
  id: string;
  title: string;
  description: string;
};

export function QuizCard({ quiz }: { quiz: Quiz }) {
  return (
    <div className="card">
      <h2>{quiz.title}</h2>

      <p style={{ color: "var(--color-text-secondary)" }}>{quiz.description}</p>

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
