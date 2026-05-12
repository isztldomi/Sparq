import { useEffect, useState } from "react";

import { joinSessionGroup } from "../sessionGroups";

export function useSessionsConnection(sessionId: string) {
  const [connection, setConnection] = useState<any>(null);

  useEffect(() => {
    let mounted = true;

    async function setup() {
      const conn = await joinSessionGroup(sessionId);

      conn.onreconnected(async () => {
        await joinSessionGroup(sessionId);
      });

      if (mounted) {
        setConnection(conn);
      }
    }

    setup();

    return () => {
      mounted = false;
    };
  }, [sessionId]);

  return connection;
}
