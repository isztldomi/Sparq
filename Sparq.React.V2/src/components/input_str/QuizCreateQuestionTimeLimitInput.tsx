type QuizCreateQuestionTimeLimitInputProps = {
  value: number;
  onChange: (value: number) => void;
};

export function QuizCreateQuestionTimeLimitInput({
  value,
  onChange,
}: QuizCreateQuestionTimeLimitInputProps) {
  return (
    <div className="flex gap-2 bg-[var(--surface-5)] p-4 rounded-lg">
      Time Limit (seconds):
      <input
        type="number"
        min={10}
        max={7200}
        className="w-full"
        placeholder="Time limit (seconds)"
        value={value}
        onChange={(e) => onChange(Number(e.target.value))}
      />
    </div>
  );
}
