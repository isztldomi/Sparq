import { store } from "@/app/store";
import { sessionApi } from "@/features/session/sessionApi";
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
 * 👉 Ez akkor fut le, amikor a backend ezt hívja:
 *    Clients.Group(...).SessionStart(sessionId)
 */
export function registerSessionStartHandler({ connection }: Params) {
  /**
   * 👉 Ez a függvény fog lefutni minden "SessionStart" eventnél
   */
  const handler = (sessionId: string) => {
    console.log("SESSION START:", sessionId);

    store.dispatch(
      sessionApi.util.invalidateTags([{ type: "Session", id: sessionId }]),
    );
  };

  /**
   * 👉 Feliratkozás a SignalR eventre
   */
  connection.on("SessionStart", handler);

  /**
   * 👉 Cleanup function
   * Fontos: reconnect / unmount esetén ne maradjon bent listener
   */
  return () => {
    connection.off("SessionStart", handler);
  };
}
