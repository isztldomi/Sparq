type GreenButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
};

export const GreenButton = ({
  children,
  onClick,
  className = "",
}: GreenButtonProps) => {
  return (
    <button
      onClick={onClick}
      className={`bg-[var(--success-bg)] text-[var(--success-text)] hover:bg-[var(--success-text)] hover:text-[var(--success-bg)] transition rounded-lg ${className}`}
    >
      {children}
    </button>
  );
};
