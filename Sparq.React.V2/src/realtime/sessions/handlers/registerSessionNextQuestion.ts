import { sessionApi } from "@/features/session/sessionApi";
import { store } from "@/app/store";
import type { HubConnection } from "@microsoft/signalr";

type Params = {
  connection: HubConnection;
};

export function registerSessionNextQuestionHandler({ connection }: Params) {
  const handler = (sessionId: string) => {
    console.log("SESSION NEXT QUESTION EVENT RECEIVED:", sessionId);

    store.dispatch(
      sessionApi.util.invalidateTags([
        { type: "Question", id: `${sessionId}-current-without-result` },
        { type: "Question", id: `${sessionId}-current-with-result` },
        { type: "Session", id: sessionId },
      ]),
    );
  };

  connection.on("SessionNextQuestion", handler);

  return () => {
    connection.off("SessionNextQuestion", handler);
  };
}
