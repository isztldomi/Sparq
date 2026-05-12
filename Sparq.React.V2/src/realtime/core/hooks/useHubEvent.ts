import { useEffect } from "react";
import type { HubConnection } from "@microsoft/signalr";

type Handler = (...args: any[]) => void;

type Props = {
  connection: HubConnection | null;
  event: string;
  handler: Handler;
};

export function useHubEvent({ connection, event, handler }: Props) {
  useEffect(() => {
    if (!connection) return;

    connection.on(event, handler);

    return () => {
      connection.off(event, handler);
    };
  }, [connection, event, handler]);
}
