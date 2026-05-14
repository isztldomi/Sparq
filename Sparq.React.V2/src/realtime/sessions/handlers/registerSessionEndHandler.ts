import { sessionApi } from "@/features/session/sessionApi";
import { store } from "@/app/store";
import type { HubConnection } from "@microsoft/signalr";

type Params = {
  connection: HubConnection;
};

export function registerSessionEndHandler({ connection }: Params) {
  const handler = (sessionId: string) => {
    console.log("SESSION END EVENT RECEIVED:", sessionId);

    store.dispatch(
      sessionApi.util.invalidateTags([
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
      ]),
    );
  };

  connection.on("SessionEnd", handler);

  return () => {
    connection.off("SessionEnd", handler);
  };
}
