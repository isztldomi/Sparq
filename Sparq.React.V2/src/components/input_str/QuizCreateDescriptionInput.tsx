type QuizCreateDescriptionInputProps = {
  value: string;
  onChange: (value: string) => void;
};

export function QuizCreateDescriptionInput({
  value,
  onChange,
}: QuizCreateDescriptionInputProps) {
  return (
    <div className="flex gap-2 bg-[var(--surface-4)] p-4 rounded-lg">
      <textarea
        placeholder="Quiz description"
        className="w-full"
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
}
