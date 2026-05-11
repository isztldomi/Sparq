type RedButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  disabled?: boolean;
  className?: string;
};

export const RedButton = ({
  children,
  onClick,
  disabled = false,
  className = "",
}: RedButtonProps) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={`bg-[var(--error-bg)] text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)] transition rounded-lg ${className}`}
    >
      {children}
    </button>
  );
};
