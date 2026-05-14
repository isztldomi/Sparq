import { useEffect, useMemo, useState } from "react";
import type { HubConnection } from "@microsoft/signalr";

import { SessionRealtimeContext } from "../context/SessionRealtimeContext";

import { createSessionConnection } from "../handlers/createSessionConnection";
import { registerSessionConnectionLifecycle } from "../handlers/registerSessionConnectionLifecycle";
import { registerSessionStartHandler } from "../handlers/registerSessionStartHandler";
import { registerSessionNextQuestionHandler } from "../handlers/registerSessionNextQuestion";
import { registerSessionEndHandler } from "../handlers/registerSessionEndHandler";

type Props = {
  sessionId: string;
  children: React.ReactNode;
};

export function SessionRealtimeProvider({ sessionId, children }: Props) {
  const [connection, setConnection] = useState<HubConnection | null>(null);

  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    let mounted = true;

    let cleanupSessionStart: (() => void) | undefined;
    let cleanupSessionNextQuestion: (() => void) | undefined;
    let cleanupSessionEnd: (() => void) | undefined;

    async function setup() {
      const conn = await createSessionConnection(sessionId);

      registerSessionConnectionLifecycle({
        connection: conn,
        sessionId,
        setIsConnected,
      });

      cleanupSessionStart = registerSessionStartHandler({
        connection: conn,
      });

      cleanupSessionNextQuestion = registerSessionNextQuestionHandler({
        connection: conn,
      });

      cleanupSessionEnd = registerSessionEndHandler({
        connection: conn,
      });

      if (!mounted) return;

      setConnection(conn);
      setIsConnected(conn.state === "Connected");
    }

    setup();

    return () => {
      mounted = false;

      // event listener törlés
      cleanupSessionStart?.();
      cleanupSessionNextQuestion?.();
      cleanupSessionEnd?.();
      connection?.stop().catch(() => {});
    };
  }, [sessionId]);

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
