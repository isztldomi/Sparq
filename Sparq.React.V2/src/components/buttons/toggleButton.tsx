import { ChevronDown, ChevronUp } from "lucide-react";

type ToggleButtonProps = {
  isOpen: boolean;
  onClick: () => void;
  className?: string;
};

export function ToggleButton({
  isOpen,
  onClick,
  className = "",
}: ToggleButtonProps) {
  return (
    <button
      onClick={onClick}
      type="button"
      aria-label={isOpen ? "Collapse" : "Expand"}
      className={`text-gray-300 hover:text-white transition ${className}`}
    >
      {isOpen ? (
        <ChevronUp className="w-5 h-5" />
      ) : (
        <ChevronDown className="w-5 h-5" />
      )}
    </button>
  );
}
