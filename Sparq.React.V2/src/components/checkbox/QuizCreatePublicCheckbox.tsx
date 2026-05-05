type QuizCreatePublicCheckboxProps = {
  checked: boolean;
  onChange: (value: boolean) => void;
};

export function QuizCreatePublicCheckbox({
  checked,
  onChange,
}: QuizCreatePublicCheckboxProps) {
  return (
    <label className="flex items-center gap-2 bg-[var(--surface-4)] p-4 rounded-lg">
      <input
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
      />
      Public
    </label>
  );
}
