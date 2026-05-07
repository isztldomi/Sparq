import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGetMyQuizzesQuery } from "@/features/quiz/quizApi";
import type { MyQuizListDto } from "@/features/quiz/quizTypes";
import { GreenButton } from "../buttons/greenButton";
import { InlineLoading } from "@/components/loadings/InlineLoading";
import { YellowButton } from "../buttons/yellowButton";

export function MyQuizzesListContainer() {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data, isLoading, isFetching } = useGetMyQuizzesQuery({
    page,
    pageSize,
  });

  const items = data?.items ?? [];
  const totalCount = data?.totalCount ?? 0;
  const totalPages = Math.ceil(totalCount / pageSize);

  if (isLoading) {
    return <InlineLoading />;
  }

  return (
    <div className="mt-6 space-y-4">
      {isFetching && <InlineLoading />}

      {items.length === 0 ? (
        <p className="text-[var(--text-h)]">No quizzes found.</p>
      ) : (
        <ul className="space-y-2">
          {items.map((quiz: MyQuizListDto) => (
            <li
              key={quiz.id}
              className="flex justify-between items-center gap-3 p-5 rounded-lg bg-[var(--surface-4)]"
            >
              <div className="flex flex-col max-w-40">
                <p className="text-2xl break-words">
                  {quiz.lastSnapshot?.title ?? "Untitled quiz"}
                </p>

                <p className="text-sm text-[var(--text-muted)] break-words">
                  {quiz.lastSnapshot?.description}
                </p>
              </div>
              <div className="flex flex-wrap items-center gap-3">
                <GreenButton className="p-3 text-lg">Sessions</GreenButton>
                <YellowButton
                  className="p-3 text-lg"
                  onClick={() => navigate(`/my-quizzes/${quiz.id}/modify`)}
                >
                  Modify
                </YellowButton>
              </div>
            </li>
          ))}
        </ul>
      )}

      <div className="flex flex-wrap gap-5 items-center justify-center">
        <GreenButton
          disabled={page === 1}
          onClick={() => setPage((p) => p - 1)}
          className="w-20 h-10"
        >
          Prev
        </GreenButton>

        <span className="bg-[var(--error-bg)] text-[var(--error-text)] w-25 h-10 rounded-lg flex items-center justify-center">
          Page {page} / {totalPages || 1}
        </span>

        <GreenButton
          disabled={page >= totalPages}
          onClick={() => setPage((p) => p + 1)}
          className="w-20 h-10"
        >
          Next
        </GreenButton>
      </div>
    </div>
  );
}
