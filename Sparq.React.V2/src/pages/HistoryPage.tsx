import { useState } from "react";
import { useHistoryQuery } from "@/features/session/sessionApi";
import { BasePaginatedList } from "@/components/paginatedLists/BasePaginatedList";
import type { MySessionListDto } from "@/features/session/sessionTypes";
import { GreenButton } from "@/components/buttons/greenButton";
import { useNavigate } from "react-router-dom";

export function HistoryPage() {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data, isLoading, isFetching, isError } = useHistoryQuery({
    page,
    pageSize,
  });

  if (isError) {
    return <div>Failed to load history</div>;
  }

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold">Session History</h1>

      <BasePaginatedList<MySessionListDto>
        items={data?.items ?? []}
        isLoading={isLoading}
        isFetching={isFetching}
        page={page}
        pageSize={pageSize}
        totalCount={data?.totalCount ?? 0}
        onPageChange={({ page }) => setPage(Number(page))}
        emptyContent={
          <p className="text-[var(--text-muted)]">No sessions found.</p>
        }
        renderItem={(session) => (
          <div className="flex justify-between items-center p-4 rounded-lg bg-[var(--surface-4)]">
            <div className="flex flex-col">
              <span className="text-lg font-medium">
                {session.snapshotTitle}
              </span>
            </div>
            <GreenButton
              className="w-20 h-10"
              onClick={() => {
                navigate(`${session.sessionId}`);
              }}
            >
              Open
            </GreenButton>
          </div>
        )}
      />
    </div>
  );
}
