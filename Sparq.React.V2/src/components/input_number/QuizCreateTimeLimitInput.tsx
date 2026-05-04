type QuizCreateTimeLimitInputProps = {
  value: number | null;
  onChange: (value: number) => void;
};

export function QuizCreateTimeLimitInput({
  value,
  onChange,
}: QuizCreateTimeLimitInputProps) {
  return (
    <div className="flex gap-2 bg-[var(--surface-4)] p-4 rounded-lg">
      Time Limit (seconds):
      <input
        type="number"
        min={10}
        max={7200}
        className="w-full"
        placeholder="Time limit (seconds)"
        value={value ?? ""}
        onChange={(e) => {
          const v = e.target.value;
          onChange(v === "" ? 10 : Number(v));
        }}
      />
    </div>
  );
}
