import { InlineLoading } from "@/components/loadings/InlineLoading";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { GreenButton } from "@/components/buttons/greenButton";

type BasePaginatedListProps<T> = {
  items: T[];
  isLoading: boolean;
  isFetching?: boolean;

  page: number;
  pageSize: number;
  totalCount: number;

  onPageChange: (params: { page: string; pageSize: string }) => void;

  renderItem: (item: T) => React.ReactNode;

  emptyContent?: React.ReactNode;
};

export function BasePaginatedList<T>({
  items,
  isLoading,
  isFetching,
  page,
  pageSize,
  totalCount,
  onPageChange,
  renderItem,
  emptyContent,
}: BasePaginatedListProps<T>) {
  const totalPages = Math.ceil(totalCount / pageSize);

  if (isLoading) return <LoadingIndicator />;

  return (
    <div className="mt-6 space-y-4">
      {isFetching && <InlineLoading />}

      {items.length === 0 ? (
        (emptyContent ?? <p>No items found.</p>)
      ) : (
        <ul className="space-y-2">
          {items.map((item, idx) => (
            <li key={idx}>{renderItem(item)}</li>
          ))}
        </ul>
      )}

      {totalCount > pageSize && (
        <div className="flex flex-wrap gap-5 items-center justify-center">
          <GreenButton
            className="w-30 h-10"
            disabled={page === 1}
            onClick={() =>
              onPageChange({
                page: String(page - 1),
                pageSize: String(pageSize),
              })
            }
          >
            Prev
          </GreenButton>

          <span className="bg-[var(--error-bg)] text-[var(--error-text)] w-30 h-10 rounded-lg flex items-center justify-center">
            Page {page} / {totalPages || 1}
          </span>

          <GreenButton
            className="w-30 h-10"
            disabled={page >= totalPages}
            onClick={() =>
              onPageChange({
                page: String(page + 1),
                pageSize: String(pageSize),
              })
            }
          >
            Next
          </GreenButton>
        </div>
      )}
    </div>
  );
}
