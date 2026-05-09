import { useNavigate } from "react-router-dom";
import { BasePaginatedList } from "@/components/paginatedLists/BasePaginatedList";
import { useGetAllPublicWaitingSessionsQuery } from "@/features/session/sessionApi";
import type { SessionPublicWaitingListDto } from "@/features/session/sessionTypes";
import { GreenButton } from "../buttons/greenButton";

type PublicActiveSessionsListContainerProps = {
  page: number;
  pageSize: number;
  onPageChange: (params: Record<string, string>) => void;
};

export function PublicActiveSessionsListContainer({
  page,
  pageSize,
  onPageChange,
}: PublicActiveSessionsListContainerProps) {
  const navigate = useNavigate();

  const { data, isLoading, isFetching, isError } =
    useGetAllPublicWaitingSessionsQuery({
      page,
      pageSize,
    });

  if (isError) {
    return <p>Failed to load sessions.</p>;
  }

  return (
    <BasePaginatedList<SessionPublicWaitingListDto>
      items={data?.items ?? []}
      isLoading={isLoading}
      isFetching={isFetching}
      page={page}
      pageSize={pageSize}
      totalCount={data?.totalCount ?? 0}
      onPageChange={onPageChange}
      emptyContent={
        <p className="text-[var(--text-h)]">No public sessions found.</p>
      }
      renderItem={(session) => (
        <div className="flex flex-wrap justify-between items-center gap-3 p-5 rounded-lg bg-[var(--surface-4)]">
          {/* LEFT: info */}
          <div className="flex flex-col max-w-60">
            <p className="text-2xl break-words">
              {session.snapshot?.title ?? "Untitled session"}
            </p>

            <p className="text-sm text-[var(--text-muted)] break-words">
              {session.snapshot?.description ?? "No description"}
            </p>
          </div>

          {/* MIDDLE: session id */}
          <div className="text-xs text-[var(--text-muted)]">
            ID: {session.id}
          </div>

          {/* RIGHT: join */}
          <div>
            <GreenButton
              className="w-30 h-10"
              onClick={() => navigate(`/session/${session.id}/join`)}
            >
              Join
            </GreenButton>
          </div>
        </div>
      )}
    />
  );
}
