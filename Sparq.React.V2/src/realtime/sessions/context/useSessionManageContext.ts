import { useContext } from "react";
import { SessionManageContext } from "./SessionManageContext";

export function useSessionManageContext() {
  const ctx = useContext(SessionManageContext);

  if (!ctx) {
    throw new Error("Must be inside provider");
  }

  return ctx;
}
