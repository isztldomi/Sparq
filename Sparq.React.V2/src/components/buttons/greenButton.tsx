type GreenButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
  type?: "button" | "submit" | "reset";
  disabled?: boolean;
};

export const GreenButton = ({
  children,
  onClick,
  className = "",
  type = "button",
  disabled = false,
}: GreenButtonProps) => {
  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      className={` bg-[var(--success-bg)] text-[var(--success-text)]
      transition rounded-lg 
      ${disabled ? "opacity-50 cursor-not-allowed" : " hover:bg-[var(--success-text)] hover:text-[var(--success-bg)]"} 
      ${className}`}
    >
      {children}
    </button>
  );
};
