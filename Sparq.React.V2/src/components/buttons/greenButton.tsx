type GreenButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
  type?: "button" | "submit" | "reset";
};

export const GreenButton = ({
  children,
  onClick,
  className = "",
  type = "button",
}: GreenButtonProps) => {
  return (
    <button
      type={type}
      onClick={onClick}
      className={`bg-[var(--success-bg)] text-[var(--success-text)] hover:bg-[var(--success-text)] hover:text-[var(--success-bg)] transition rounded-lg ${className}`}
    >
      {children}
    </button>
  );
};
