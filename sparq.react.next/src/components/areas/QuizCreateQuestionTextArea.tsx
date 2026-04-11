type QuizCreateQuestionTextAreaProps = {
  value: string;
  onChange: (value: string) => void;
};

export function QuizCreateQuestionTextArea({
  value,
  onChange,
}: QuizCreateQuestionTextAreaProps) {
  return (
    <div className="w-full bg-[var(--surface-5)] p-4 rounded-lg">
      <textarea
        placeholder="Question text"
        value={value}
        className="w-full"
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
}
