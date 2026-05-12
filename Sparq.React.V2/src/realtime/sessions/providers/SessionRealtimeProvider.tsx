import { useEffect, useMemo, useState } from "react";

import type { HubConnection } from "@microsoft/signalr";

import { SessionRealtimeContext } from "../context/SessionRealtimeContext";

import { joinSessionGroup } from "../sessionGroups";

type Props = {
  sessionId: string;

  children: React.ReactNode;
};

export function SessionRealtimeProvider({ sessionId, children }: Props) {
  const [connection, setConnection] = useState<HubConnection | null>(null);

  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    let mounted = true;

    async function setup() {
      const conn = await joinSessionGroup(sessionId);

      conn.onreconnected(async () => {
        await joinSessionGroup(sessionId);
      });

      conn.onclose(() => {
        setIsConnected(false);
      });

      conn.onreconnecting(() => {
        setIsConnected(false);
      });

      if (!mounted) return;

      setConnection(conn);

      setIsConnected(conn.state === "Connected");
    }

    setup();

    return () => {
      mounted = false;
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
