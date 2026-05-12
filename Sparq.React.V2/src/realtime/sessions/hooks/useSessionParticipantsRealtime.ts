import { useAppDispatch } from "@/app/hooks";

import { participantApi } from "@/features/participant/participantApi";

import { useSessionsConnection } from "./useSessionsConnection";

import { useHubEvent } from "../../core/hooks/useHubEvent";

import { SESSION_EVENTS } from "../sessionEvents";

export function useSessionParticipantsUpdated(sessionId: string) {
  const dispatch = useAppDispatch();

  const connection = useSessionsConnection(sessionId);

  useHubEvent({
    connection,

    event: SESSION_EVENTS.PARTICIPANTS_UPDATED,

    handler: () => {
      dispatch(
        participantApi.util.invalidateTags([
          {
            type: "Participant",
            id: sessionId,
          },
        ]),
      );
    },
  });
}
