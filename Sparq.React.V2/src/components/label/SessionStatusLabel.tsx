type SessionStatusLabelProps = {
  children: React.ReactNode;
  variant?: "success" | "warning" | "error" | "info" | "neutral";
  className?: string;
};

const variants = {
  success: "bg-green-500/15 text-green-400 border border-green-500/30",

  warning: "bg-yellow-500/15 text-yellow-300 border border-yellow-500/30",

  error: "bg-red-500/15 text-red-400 border border-red-500/30",

  info: "bg-blue-500/15 text-blue-400 border border-blue-500/30",

  neutral: "bg-white/10 text-white/70 border border-white/10",
};

export function SessionStatusLabel({
  children,
  variant = "neutral",
  className = "",
}: SessionStatusLabelProps) {
  return (
    <span
      className={`
        inline-flex items-center justify-center
        px-3 py-1 rounded-full
        text-sm font-medium
        ${variants[variant]}
        ${className}
      `}
    >
      {children}
    </span>
  );
}
