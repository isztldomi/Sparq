type QuizCreatePinCodeInputProps = {
  value: string;
  onChange: (value: string) => void;
};

export function QuizCreatePinCodeInput({
  value,
  onChange,
}: QuizCreatePinCodeInputProps) {
  return (
    <div className="flex gap-2 bg-[var(--surface-4)] p-4 rounded-lg">
      Pin Code:
      <input
        type="text"
        className="w-full"
        placeholder="Pin code"
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
}
