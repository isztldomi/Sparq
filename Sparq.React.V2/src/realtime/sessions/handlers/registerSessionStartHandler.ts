import { store } from "@/app/store";
import { sessionApi } from "@/features/session/sessionApi";
import type { HubConnection } from "@microsoft/signalr";

type Params = {
  connection: HubConnection;
};

export function registerSessionStartHandler({ connection }: Params) {
  const handler = (sessionId: string) => {
    console.log("SESSION START:", sessionId);

    store.dispatch(
      sessionApi.util.invalidateTags([
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
      ]),
    );
  };

  connection.on("SessionStart", handler);

  return () => {
    connection.off("SessionStart", handler);
  };
}
