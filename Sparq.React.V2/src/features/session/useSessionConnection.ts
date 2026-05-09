import { useEffect } from "react";
import { getSessionConnection } from "./sessionConnection";

export function useSessionConnection(sessionId?: string) {
  useEffect(() => {
    if (!sessionId) return;

    const connection = getSessionConnection();

    async function connect() {
      if (connection.state === "Disconnected") {
        await connection.start();
      }

      await connection.invoke("JoinSession", sessionId);
    }

    connect();

    return () => {
      connection.invoke("LeaveSession", sessionId);
    };
  }, [sessionId]);
}
