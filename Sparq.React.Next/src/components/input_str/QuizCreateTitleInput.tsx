type QuizCreateTitleInputProps = {
  value: string;
  onChange: (value: string) => void;
};

export function QuizCreateTitleInput({
  value,
  onChange,
}: QuizCreateTitleInputProps) {
  return (
    <div className="flex gap-2 bg-[var(--surface-4)] p-4 rounded-lg">
      <input
        placeholder="Quiz title"
        className="w-full"
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
}
