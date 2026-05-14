import { sessionApi } from "@/features/session/sessionApi";
import { store } from "@/app/store";
import type { HubConnection } from "@microsoft/signalr";

/**
 * PARAMS:
 * - connection: a SignalR kapcsolat (HubConnection)
 */
type Params = {
  connection: HubConnection;
};

/**
 * SessionStart event handler
 *
 * Ez akkor fut le, amikor a backend ezt hívja:
 *    Clients.Group(...).SessionStart(sessionId)
 */
export function registerSessionEndHandler({ connection }: Params) {
  /**
   * Ez a függvény fog lefutni minden "SessionStart" eventnél
   */
  const handler = (sessionId: string) => {
    console.log("SESSION END EVENT RECEIVED:", sessionId);

    store.dispatch(
      sessionApi.util.invalidateTags([
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
      ]),
    );
  };

  /**
   * Feliratkozás a SignalR eventre
   */
  connection.on("SessionEnd", handler);

  /**
   * 👉 Cleanup function
   * Fontos: reconnect / unmount esetén ne maradjon bent listener
   */
  return () => {
    connection.off("SessionEnd", handler);
  };
}
