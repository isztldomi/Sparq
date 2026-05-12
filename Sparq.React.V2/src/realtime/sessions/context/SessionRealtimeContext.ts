import { createContext } from "react";

import type { SessionRealtimeContextValue } from "./types";

export const SessionRealtimeContext =
  createContext<SessionRealtimeContextValue | null>(null);
