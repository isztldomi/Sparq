import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { useGetParticipantsBySessionIdQuery } from "@/features/participant/participantApi";
import { useSessionRealtime } from "@/realtime/hooks/useSessionRealtime";

type Props = {
  sessionId: string;
};

export function SessionManageParticipantsContainer({ sessionId }: Props) {
  const {
    data: participantData,
    isLoading,
    isError,
    refetch,
  } = useGetParticipantsBySessionIdQuery({
    sessionId,
  });

  useSessionRealtime({
    sessionId,
    onParticipantsUpdated: async () => {
      await refetch();
    },
  });

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
      <div className="flex items-center justify-between">
        <h2 className="text-lg">Participants ({participantData.length})</h2>

        <div className="text-sm opacity-70">Realtime synced</div>
      </div>

      {participantData.length === 0 ? (
        <div className="rounded-lg bg-[var(--surface-5)] p-4">
          No participants yet.
        </div>
      ) : (
        <div className="flex flex-col gap-2">
          {participantData.map((participant) => (
            <div
              key={participant.id}
              className="flex items-center justify-between rounded-lg bg-[var(--surface-5)] p-3"
            >
              <div className="flex flex-col">
                <span className="font-medium">{participant.displayName}</span>

                <span className="text-xs opacity-60">{participant.id}</span>
              </div>

              <div className="text-sm opacity-70">Joined</div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
