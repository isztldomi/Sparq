type QuizCreateQuestionTitleInputProps = {
  value: string;
  onChange: (value: string) => void;
};

export function QuizCreateQuestionTitleInput({
  value,
  onChange,
}: QuizCreateQuestionTitleInputProps) {
  return (
    <input
      placeholder="Question title"
      value={value}
      className="w-full"
      onChange={(e) => onChange(e.target.value)}
    />
  );
}
