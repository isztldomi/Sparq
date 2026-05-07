import { Cat } from "lucide-react";

export function InlineLoading() {
  return (
    <div className="flex items-center justify-center py-4">
      <Cat className="w-10 h-10 loader-anim" />
    </div>
  );
}
