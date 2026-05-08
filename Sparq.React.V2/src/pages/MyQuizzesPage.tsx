import { GreenButton } from "@/components/buttons/greenButton";
import { MyQuizzesListContainer } from "@/components/containers/MyQuizzesListContainer";
import { useNavigate, useSearchParams } from "react-router-dom";

export function MyQuizzesPage() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();

  const page = Number(searchParams.get("page") ?? 1);
  const pageSize = Number(searchParams.get("pageSize") ?? 10);

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">My Quizzes</h1>

        <GreenButton
          className="w-30 h-10"
          onClick={() => navigate("/my-quizzes/create")}
        >
          Quiz Create
        </GreenButton>
      </div>

      <MyQuizzesListContainer
        page={page}
        pageSize={pageSize}
        onPageChange={setSearchParams}
      />
    </div>
  );
}
