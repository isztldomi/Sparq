import { useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { BasePaginatedList } from "@/components/paginatedLists/BasePaginatedList";
import { useGetQuizSessionsByIdQuery } from "@/features/quiz/quizApi";
import {
  SessionStatus,
  type MyQuizSessionsListDto,
} from "@/features/session/sessionTypes";
import { SessionStatusLabel } from "@/components/label/SessionStatusLabel";
import { GreenButton } from "../buttons/greenButton";
import { useActivateForWaitingSessionMutation } from "@/features/session/sessionApi";
import { InlineLoading } from "../loadings/InlineLoading";

type Props = {
  page: number;
  pageSize: number;
  onPageChange: (params: Record<string, string>) => void;
};

export function QuizSessionsListContainer({
  page,
  pageSize,
  onPageChange,
}: Props) {
  const navigate = useNavigate();
  const { quizId } = useParams();
  const [activateSession, { isLoading: isActivating }] =
    useActivateForWaitingSessionMutation();
  const [activatingId, setActivatingId] = useState<string | null>(null);

  const { data, isLoading, isFetching, isError } = useGetQuizSessionsByIdQuery(
    {
      id: quizId as string,
      page,
      pageSize,
    },
    {
      skip: !quizId,
    },
  );

  if (isError) {
    return <Navigate to="/my-quizzes/notFound" replace />;
  }

  return (
    <BasePaginatedList<MyQuizSessionsListDto>
      items={data?.items ?? []}
      isLoading={isLoading}
      isFetching={isFetching}
      page={page}
      pageSize={pageSize}
      totalCount={data?.totalCount ?? 0}
      onPageChange={onPageChange}
      emptyContent={<p className="text-[var(--text-h)]">No sessions found.</p>}
      renderItem={(session) => (
        <div className="flex flex-wrap justify-between items-center gap-3 p-5 rounded-lg bg-[var(--surface-4)]">
          {activatingId === session.id ? (
            <div>
              <InlineLoading />
            </div>
          ) : (
            <>
              <div className="flex flex-col max-w-40">
                <p className="text-2xl break-words">
                  {session.snapshot.title ?? "Untitled quiz"}
                </p>

                <p className="text-sm text-[var(--text-muted)] break-words">
                  {session.snapshot.description}
                </p>
                <p className="text-xs text-[var(--text-muted)] break-words">
                  {session.id}
                </p>
              </div>
              <div>
                {session.status === SessionStatus.Created && (
                  <SessionStatusLabel variant="neutral">
                    Not Started
                  </SessionStatusLabel>
                )}

                {session.status === SessionStatus.Waiting && (
                  <SessionStatusLabel variant="warning">
                    Waiting
                  </SessionStatusLabel>
                )}

                {session.status === SessionStatus.Running && (
                  <SessionStatusLabel variant="error">
                    Running
                  </SessionStatusLabel>
                )}

                {session.status === SessionStatus.Finished && (
                  <SessionStatusLabel variant="info">Ended</SessionStatusLabel>
                )}
              </div>
              <div>
                {session.status === SessionStatus.Created && (
                  <GreenButton
                    className="w-30 h-10"
                    onClick={async () => {
                      try {
                        setActivatingId(session.id);
                        await activateSession(session.id).unwrap();
                      } finally {
                        setActivatingId(null);
                      }
                    }}
                    disabled={isActivating}
                  >
                    {isActivating ? "Activating..." : "Activate session"}
                  </GreenButton>
                )}
                {session.status === SessionStatus.Waiting && (
                  <GreenButton
                    className="w-30 h-10"
                    onClick={() => navigate(`/session/${session.id}`)}
                  >
                    Join session
                  </GreenButton>
                )}
              </div>
            </>
          )}
        </div>
      )}
    />
  );
}
