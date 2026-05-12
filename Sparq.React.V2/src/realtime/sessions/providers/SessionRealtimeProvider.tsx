import { useEffect, useMemo, useState } from "react";
import type { HubConnection } from "@microsoft/signalr";

import { SessionRealtimeContext } from "../context/SessionRealtimeContext";

import { createSessionConnection } from "../handlers/createSessionConnection";
import { registerSessionConnectionLifecycle } from "../handlers/registerSessionConnectionLifecycle";
import { registerSessionStartHandler } from "../handlers/registerSessionStartHandler";

type Props = {
  sessionId: string;
  children: React.ReactNode;
};

export function SessionRealtimeProvider({ sessionId, children }: Props) {
  const [connection, setConnection] = useState<HubConnection | null>(null);

  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    let mounted = true;

    // cleanup függvények tárolása
    // (SignalR eventekhez)
    let cleanupSessionStart: (() => void) | undefined;

    async function setup() {
      // 1. Connection létrehozása
      const conn = await createSessionConnection(sessionId);

      // 2. Lifecycle (reconnect, close stb.)
      registerSessionConnectionLifecycle({
        connection: conn,
        sessionId,
        setIsConnected,
      });

      // 3. SESSION START event handler regisztrálása
      cleanupSessionStart = registerSessionStartHandler({
        connection: conn,
      });

      // 4. state update (csak ha még mounted)
      if (!mounted) return;

      setConnection(conn);
      setIsConnected(conn.state === "Connected");
    }

    setup();

    // CLEANUP
    return () => {
      mounted = false;

      // event listener törlés
      cleanupSessionStart?.();
    };
  }, [sessionId]);

  // context value
  const value = useMemo(
    () => ({
      sessionId,
      connection,
      isConnected,
    }),
    [sessionId, connection, isConnected],
  );

  return (
    <SessionRealtimeContext.Provider value={value}>
      {children}
    </SessionRealtimeContext.Provider>
  );
}
