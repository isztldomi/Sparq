import { Navigate, useParams } from "react-router-dom";
import { BasePaginatedList } from "@/components/paginatedLists/BasePaginatedList";
import { useGetQuizSessionsByIdQuery } from "@/features/quiz/quizApi";
import type { MyQuizSessionsListDto } from "@/features/session/sessionTypes";
import { SessionStatusLabel } from "@/components/label/SessionStatusLabel";

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
  const { quizId } = useParams();

  const id = quizId ?? null;

  const { data, isLoading, isFetching, isError } = useGetQuizSessionsByIdQuery(
    { id, page, pageSize },
    {
      skip: quizId,
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
        <div className="flex justify-between items-center gap-3 p-5 rounded-lg bg-[var(--surface-4)]">
          <div className="flex flex-col max-w-40">
            <p className="text-2xl break-words">
              {session.snapshot.title ?? "Untitled quiz"}
            </p>

            <p className="text-sm text-[var(--text-muted)] break-words">
              {session.snapshot.description}
            </p>
          </div>
          {session.isWaiting && (
            <div>
              <SessionStatusLabel variant="warning">Waiting</SessionStatusLabel>
            </div>
          )}
        </div>
      )}
    />
  );
}
