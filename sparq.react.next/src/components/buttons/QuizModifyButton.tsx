type QuizModifyButtonProps = {
  onClick?: () => void;
};

export function QuizModifyButton({ onClick }: QuizModifyButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-3 py-3 rounded-xl bg-[var(--warning-bg)] text-sm text-[var(--warning-text)] hover:bg-[var(--warning-text)] hover:text-[var(--warning-bg)] transition"
    >
      Modify Quiz
    </button>
  );
}
