import { QuizCard } from "@/components/cards/QuizCard";

type Quiz = {
  id: string;
  title: string;
  description: string;
};

export function QuizContainer({ quizzes }: { quizzes: Quiz[] }) {
  return (
    <div style={{ display: "grid", gap: "1rem" }}>
      {quizzes.map((quiz) => (
        <QuizCard key={quiz.id} quiz={quiz} />
      ))}
    </div>
  );
}
