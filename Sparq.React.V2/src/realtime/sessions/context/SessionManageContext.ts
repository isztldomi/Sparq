import { createContext } from "react";
import type { SessionManageContextValue } from "./types";

export const SessionManageContext =
  createContext<SessionManageContextValue | null>(null);
