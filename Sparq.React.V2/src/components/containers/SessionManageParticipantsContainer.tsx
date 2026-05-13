import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

import {
  useDeleteParticipantFromSessionByIdMutation,
  useGetParticipantsBySessionIdQuery,
} from "@/features/participant/participantApi";

import { useSessionParticipantsUpdated } from "@/realtime/sessions/hooks/useSessionParticipantsUpdated";

import { RedButton } from "../buttons/redButton";

import { useSessionManageContext } from "@/realtime/sessions/context/useSessionManageContext";

import { SessionStatus } from "@/features/session/sessionTypes";

export function SessionManageParticipantsContainer() {
  const { sessionId, sessionData } = useSessionManageContext();

  useSessionParticipantsUpdated();

  const {
    data: participantData,
    isLoading,
    isError,
  } = useGetParticipantsBySessionIdQuery({
    sessionId,
  });

  const [deleteParticipant, { isLoading: isDeleting }] =
    useDeleteParticipantFromSessionByIdMutation();

  if (isLoading) {
    return <LoadingIndicator />;
  }

  if (isError || !participantData) {
    return (
      <div className="w-full rounded-lg bg-[var(--surface-4)] p-5">
        Failed to load participants.
      </div>
    );
  }

  return (
    <div className="w-full rounded-lg bg-[var(--surface-4)] p-5 flex flex-col gap-4">
      <h2 className="text-lg font-semibold">
        Participants ({participantData.length})
      </h2>

      {participantData.length === 0 ? (
        <div className="flex items-center justify-between rounded-lg bg-[var(--surface-5)] p-3">
          No participants
        </div>
      ) : (
        participantData.map((p) => (
          <div
            key={p.id}
            className="flex items-center justify-between rounded-lg bg-[var(--surface-5)] p-3"
          >
            <div>{p.displayName}</div>

            {sessionData.status === SessionStatus.Waiting && (
              <RedButton
                className="w-20 h-10"
                onClick={() =>
                  deleteParticipant({
                    sessionId,
                    participantId: p.id,
                  })
                }
                disabled={isDeleting}
              >
                Remove
              </RedButton>
            )}
          </div>
        ))
      )}
    </div>
  );
}
