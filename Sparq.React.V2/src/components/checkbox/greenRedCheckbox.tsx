type GreenRedCheckboxProps = {
  value: boolean;
  onChange: (value: boolean) => void;
  trueLabel?: string;
  falseLabel?: string;
  className?: string;
};

export function GreenRedCheckbox({
  value,
  onChange,
  trueLabel,
  falseLabel,
  className = "",
}: GreenRedCheckboxProps) {
  return (
    <button
      type="button"
      onClick={() => onChange(!value)}
      className={`
        transition rounded-lg
        ${
          value
            ? "bg-[var(--success-bg)] text-[var(--success-text)] hover:text-[var(--success-bg)] hover:bg-[var(--success-text)]"
            : "bg-[var(--error-bg)] text-[var(--error-text)] hover:text-[var(--error-bg)] hover:bg-[var(--error-text)]"
        }
        ${className}
      `}
    >
      {value ? trueLabel : falseLabel}
    </button>
  );
}
