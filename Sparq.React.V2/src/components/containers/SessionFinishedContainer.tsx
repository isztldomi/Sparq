import { useLeaderboardSessionQuery } from "@/features/session/sessionApi";
import { InlineLoading } from "../loadings/InlineLoading";

type Props = {
  sessionId: string;
  extUserId?: string;
};

export function SessionFinishedContainer({ sessionId, extUserId }: Props) {
  const { data: leaderboard, isLoading } = useLeaderboardSessionQuery({
    sessionId,
    extUserId,
  });

  return (
    <div className="min-h-screen justify-center p-4">
      <h1 className="text-xl">Session Finished</h1>

      <div className="pt-4 bg-[var(--surface-4)] p-5 rounded-lg flex flex-col gap-4">
        <h2 className="text-lg font-semibold">Final Leaderboard</h2>

        {isLoading && <InlineLoading />}

        {!isLoading && leaderboard?.entries?.length ? (
          <div className="flex flex-col gap-2">
            {leaderboard.entries.map((e, index) => (
              <div
                key={e.participantId}
                className="flex justify-between p-3 rounded bg-[var(--surface-6)]"
              >
                <span>
                  #{index + 1} {e.displayName ?? "Unknown"}
                </span>

                <div className="flex gap-3">
                  <span>{e.totalPoints} pts</span>
                  <span>{e.correctAnswers}</span>
                </div>
              </div>
            ))}
          </div>
        ) : (
          !isLoading && <span>No results yet</span>
        )}
      </div>
    </div>
  );
}
