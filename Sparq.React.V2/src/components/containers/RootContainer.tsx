import React from "react";

export function RootContainer({ children }: { children: React.ReactNode }) {
  return (
    <div className="w-full pb-5 px-5 justify-center">
      <div className="max-w-7xl mx-auto px-4 bg-[var(--surface-3)] rounded-lg">
        {children}
      </div>
    </div>
  );
}
