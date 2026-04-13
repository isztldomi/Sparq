type QuizViewButtonProps = {
  onClick?: () => void;
};

export function QuizViewButton({ onClick }: QuizViewButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-3 py-3 rounded-xl bg-[var(--quiz-btn-bg)] text-sm text-black hover:bg-[var(--success-bg)] hover:text-[var(--success-text)] transition"
    >
      Start Quiz
    </button>
  );
}
