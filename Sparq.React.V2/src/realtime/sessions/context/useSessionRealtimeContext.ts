import { useContext } from "react";

import { SessionRealtimeContext } from "./SessionRealtimeContext";

export function useSessionRealtimeContext() {
  const context = useContext(SessionRealtimeContext);

  if (!context) {
    throw new Error(
      "useSessionRealtimeContext must be used inside SessionRealtimeProvider",
    );
  }

  return context;
}
