import { useNavigate } from "react-router-dom";
import type { MyQuizListDto } from "@/features/quiz/quizTypes";
import { BasePaginatedList } from "./BasePaginatedList";
import { GreenButton } from "../buttons/greenButton";
import { YellowButton } from "../buttons/yellowButton";

type MyQuizzesPaginatedListProps = {
  items: MyQuizListDto[];
  isLoading: boolean;
  isFetching?: boolean;

  page: number;
  pageSize: number;
  totalCount: number;

  onPageChange: (params: { page: string; pageSize: string }) => void;
};

export function MyQuizzesPaginatedList(props: MyQuizzesPaginatedListProps) {
  const navigate = useNavigate();

  return (
    <BasePaginatedList
      {...props}
      emptyContent={<p>No quizzes found.</p>}
      renderItem={(quiz) => (
        <div className="flex justify-between items-center gap-3 p-5 rounded-lg bg-[var(--surface-4)]">
          <div className="flex flex-col max-w-40">
            <p className="text-2xl break-words">
              {quiz.lastSnapshot?.title ?? "Untitled quiz"}
            </p>

            <p className="text-sm text-[var(--text-muted)] break-words">
              {quiz.lastSnapshot?.description}
            </p>
          </div>

          <div className="flex flex-wrap gap-3">
            <GreenButton
              className="w-30 h-10"
              onClick={() => navigate(`/my-quizzes/${quiz.id}/sessions`)}
            >
              Sessions
            </GreenButton>

            <YellowButton
              className="w-30 h-10"
              onClick={() => navigate(`/my-quizzes/${quiz.id}/modify`)}
            >
              Modify
            </YellowButton>
          </div>
        </div>
      )}
    />
  );
}
