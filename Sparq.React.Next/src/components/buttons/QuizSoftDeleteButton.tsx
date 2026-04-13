type QuizSoftDeleteButtonProps = {
  onClick?: () => void;
};

export function QuizSoftDeleteButton({ onClick }: QuizSoftDeleteButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-3 py-3 rounded-xl bg-[var(--error-bg)] text-sm text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)] transition"
    >
      Soft Delete Quiz
    </button>
  );
}
