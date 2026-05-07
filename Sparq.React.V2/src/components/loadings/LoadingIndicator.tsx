import { Cat } from "lucide-react";

export function LoadingIndicator() {
  return (
    <div className="min-h-screen flex items-center justify-center">
      <Cat className="w-20 h-20 loader-anim" />
    </div>
  );
}
