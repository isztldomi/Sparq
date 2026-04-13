type QuizSessionButtonProps = {
  onClick?: () => void;
};

export function QuizSessionButton({ onClick }: QuizSessionButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-3 py-3 rounded-xl bg-[var(--success-bg)] text-sm text-[var(--success-text)] hover:bg-[var(--success-text)] hover:text-[var(--success-bg)] transition"
    >
      Sessions
    </button>
  );
}
