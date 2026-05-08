import { useGetMyQuizzesQuery } from "@/features/quiz/quizApi";
import { MyQuizzesPaginatedList } from "@/components/paginatedLists/MyQuizzesPaginatedList";

type Props = {
  page: number;
  pageSize: number;
  onPageChange: (params: Record<string, string>) => void;
};

export function MyQuizzesListContainer({
  page,
  pageSize,
  onPageChange,
}: Props) {
  const { data, isLoading, isFetching } = useGetMyQuizzesQuery({
    page,
    pageSize,
  });

  return (
    <MyQuizzesPaginatedList
      items={data?.items ?? []}
      isLoading={isLoading}
      isFetching={isFetching}
      page={page}
      pageSize={pageSize}
      totalCount={data?.totalCount ?? 0}
      onPageChange={onPageChange}
    />
  );
}
