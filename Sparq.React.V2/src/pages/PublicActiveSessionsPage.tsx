import { PublicActiveSessionsListContainer } from "@/components/containers/PublicActiveSessionsListContainer";
import { useSearchParams } from "react-router-dom";

export function PublicActiveSessionsPage() {
  const [searchParams, setSearchParams] = useSearchParams();

  const page = Number(searchParams.get("page") ?? 1);
  const pageSize = Number(searchParams.get("pageSize") ?? 10);

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Sessions</h1>
      </div>

      <PublicActiveSessionsListContainer
        page={page}
        pageSize={pageSize}
        onPageChange={setSearchParams}
      />
    </div>
  );
}
