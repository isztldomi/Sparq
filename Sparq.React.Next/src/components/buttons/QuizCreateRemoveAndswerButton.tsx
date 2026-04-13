import { Minus } from "lucide-react";

type QuizCreateRemoveAnswerButtonProps = {
  onClick: () => void;
};

export function QuizCreateRemoveAndswerButton({
  onClick,
}: QuizCreateRemoveAnswerButtonProps) {
  return (
    <button
      onClick={onClick}
      className="px-3 py-3 rounded-xl bg-[var(--error-bg)] text-sm text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)] transition"
    >
      <Minus size={16} />
    </button>
  );
}
