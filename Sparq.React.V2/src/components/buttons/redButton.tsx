type RedButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
};

export const RedButton = ({
  children,
  onClick,
  className = "",
}: RedButtonProps) => {
  return (
    <button
      onClick={onClick}
      className={`bg-[var(--error-bg)] text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)] transition rounded-lg ${className}`}
    >
      {children}
    </button>
  );
};
