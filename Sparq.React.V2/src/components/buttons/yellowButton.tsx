type YellowButtonProps = {
  children?: React.ReactNode;
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
  type?: "button" | "submit" | "reset";
  disabled?: boolean;
};

export const YellowButton = ({
  children,
  onClick,
  className = "",
  type = "button",
  disabled = false,
}: YellowButtonProps) => {
  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      className={` bg-[var(--warning-bg)] text-[var(--warning-text)]
      transition rounded-lg 
      ${disabled ? "opacity-50 cursor-not-allowed" : " hover:bg-[var(--warning-text)] hover:text-[var(--warning-bg)]"} 
      ${className}`}
    >
      {children}
    </button>
  );
};
