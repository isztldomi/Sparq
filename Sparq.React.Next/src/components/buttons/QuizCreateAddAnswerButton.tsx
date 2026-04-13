type QuizCreateAddAnswerButtonProps = {
  onClick: () => void;
};

export function QuizCreateAddAnswerButton({
  onClick,
}: QuizCreateAddAnswerButtonProps) {
  return (
    <div className="flex justify-center pt-3">
      <button
        type="button"
        onClick={onClick}
        className="w-full px-3 py-3 rounded-xl bg-[var(--quiz-btn-bg)] text-sm text-black hover:bg-[var(--success-bg)] hover:text-[var(--success-text)] transition"
      >
        + Answer
      </button>
    </div>
  );
}
