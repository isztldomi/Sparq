import { Minus } from "lucide-react";

type QuizCreateRemoveQuestionButtonProps = {
  onClick: () => void;
};

export function QuizCreateRemoveQuestionButton({
  onClick,
}: QuizCreateRemoveQuestionButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-4 py-4 rounded-xl bg-[var(--error-bg)] text-sm text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)] transition"
    >
      <Minus size={16} />
    </button>
  );
}
