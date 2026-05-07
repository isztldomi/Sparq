import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { MyQuizzesListContainer } from "@/components/containers/MyQuizzesListContainer";

export function MyQuizzesPage() {
  const navigate = useNavigate();

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

      <MyQuizzesListContainer />
    </div>
  );
}
