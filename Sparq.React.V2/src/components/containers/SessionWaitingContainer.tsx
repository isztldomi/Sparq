import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { useGetParticipantsBySessionIdQuery } from "@/features/participant/participantApi";
import {
  useGetSessionPublicDataByIdQuery,
  useQuitSessionMutation,
} from "@/features/session/sessionApi";
import { useSessionRealtime } from "@/realtime/hooks/useSessionRealtime";
import { RedButton } from "../buttons/redButton";
import { useNavigate } from "react-router-dom";

type Props = {
  sessionId: string;
  extUserId?: string;
};

export function SessionWaitingContainer({ sessionId, extUserId }: Props) {
  const navigate = useNavigate();

  const {
    data: participantData,
    isLoading: isParticipantLoading,
    isError: isParticipantError,
    refetch: refetchParticipants,
  } = useGetParticipantsBySessionIdQuery({
    sessionId,
    extUserId,
  });

  const {
    data: sessionData,
    isLoading: isSessionLoading,
    isError: isSessionError,
  } = useGetSessionPublicDataByIdQuery(sessionId);

  const [quitSession, { isLoading: isQuitSessionLoading }] =
    useQuitSessionMutation();

  useSessionRealtime({
    sessionId,
    onParticipantsUpdated: async () => {
      await refetchParticipants();
    },
  });

  async function handleLeave() {
    try {
      await quitSession({
        sessionId,
        externalUserId: extUserId ?? null,
      }).unwrap();

      localStorage.removeItem(sessionId);

      navigate("/session");
    } catch (e) {
      console.error(e);
    }
  }

  if (isParticipantLoading || isSessionLoading || isQuitSessionLoading) {
    return <LoadingIndicator />;
  }

  if (
    isParticipantError ||
    isSessionError ||
    !participantData ||
    !sessionData
  ) {
    return <div>Failed to load participants.</div>;
  }

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl text-[var(--text-h)]">Waiting Room</h1>

        <RedButton className="w-30 h-10" onClick={handleLeave}>
          Leave
        </RedButton>
      </div>

      <div className="flex flex-col gap-4 my-4">
        <div className="p-4 bg-[var(--surface-4)] rounded-lg">
          <h2 className="text-[var(--text-h)] text-3xl flex flex-wrap">
            Title: {sessionData.snapshot.title}
          </h2>
        </div>

        <div className="p-4 bg-[var(--surface-4)] rounded-lg">
          <h2 className="text-[var(--text-h)] text-2xl flex flex-wrap">
            Description: {sessionData.snapshot.description}
          </h2>
        </div>
      </div>

      <div className="flex flex-col gap-3 p-4 bg-[var(--surface-4)] rounded-lg">
        <h2 className="text-2xl">Participants</h2>

        {participantData.length === 0 ? (
          <p>No participants yet.</p>
        ) : (
          <div className="flex flex-col gap-2">
            {participantData.map((participant) => (
              <div
                key={participant.id}
                className="p-3 rounded bg-[var(--surface-5)]"
              >
                {participant.displayName}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
