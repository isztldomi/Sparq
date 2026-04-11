import { useNavigate } from "react-router-dom";

export function QuizCreateButton() {
  const navigate = useNavigate();
  return (
    <div className="">
      <button
        onClick={() => navigate("/quiz/create")}
        className="px-3 py-3 rounded-xl bg-[var(--success-bg)] text-2xl text-[var(--success-text)] hover:bg-[var(--success-text)] hover:text-[var(--success-bg)] transition"
      >
        Create Quiz
      </button>
    </div>
  );
}
