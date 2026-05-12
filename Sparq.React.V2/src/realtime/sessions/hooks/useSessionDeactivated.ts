import { useNavigate } from "react-router-dom";

import { useSessionsConnection } from "./useSessionsConnection";

import { useHubEvent } from "../../core/hooks/useHubEvent";

import { SESSION_EVENTS } from "../sessionEvents";

export function useSessionDeactivated(sessionId: string) {
  const navigate = useNavigate();

  const connection = useSessionsConnection(sessionId);

  useHubEvent({
    connection,

    event: SESSION_EVENTS.SESSION_DEACTIVATED,

    handler: () => {
      localStorage.removeItem(sessionId);

      navigate("/session/notFound");
    },
  });
}
