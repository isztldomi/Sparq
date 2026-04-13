type QuizCreateQuestionPointInputProps = {
  value: number;
  onChange: (value: number) => void;
};

export function QuizCreateQuestionPointInput({
  value,
  onChange,
}: QuizCreateQuestionPointInputProps) {
  return (
    <div className="flex items-center gap-2 bg-[var(--surface-5)] p-4 rounded-lg">
      Points:
      <input
        type="number"
        value={value}
        onChange={(e) => onChange(Number(e.target.value))}
        className="w-full p-2 rounded"
        min="0"
        max="10"
      />
    </div>
  );
}
