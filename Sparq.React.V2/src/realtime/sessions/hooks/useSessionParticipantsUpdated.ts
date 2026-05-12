import { useSessionRealtimeContext } from "../context/useSessionRealtimeContext";

import { useHubEvent } from "../../core/hooks/useHubEvent";

import { SESSION_EVENTS } from "../sessionEvents";

import { participantApi } from "@/features/participant/participantApi";

import { useAppDispatch } from "@/app/hooks";

export function useSessionParticipantsUpdated() {
  const dispatch = useAppDispatch();

  const { connection, sessionId } = useSessionRealtimeContext();

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
