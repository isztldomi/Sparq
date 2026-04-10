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
      <input
        type="number"
        className="w-full"
        placeholder="Time limit (seconds)"
        value={value ?? ""}
        onChange={(e) => {
          const v = e.target.value;
          onChange(v === "" ? 0 : Number(v));
        }}
      />
    </div>
  );
}
