type QuizCreateSubmitButtonProps = {
  onClick: () => void;
};

export function QuizCreateSubmitButton({
  onClick,
}: QuizCreateSubmitButtonProps) {
  return (
    <button
      type="button"
      className="px-3 py-3 rounded-xl bg-[var(--success-bg)] text-[var(--success-text)] text-sm hover:bg-[var(--success-text)] hover:text-[var(--success-bg)] transition"
      onClick={onClick}
    >
      Submit Quiz
    </button>
  );
}
