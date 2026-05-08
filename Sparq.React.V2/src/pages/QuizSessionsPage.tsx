import { GreenButton } from "@/components/buttons/greenButton";
import { QuizSessionsListContainer } from "@/components/containers/QuizSessionsListContainer";
import { useCreateSessionMutation } from "@/features/session/sessionApi";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";

export function QuizSessionsPage() {
  const navigate = useNavigate();
  const { quizId } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();

  const id = quizId ? Number(quizId) : NaN;
  const page = Number(searchParams.get("page") ?? 1);
  const pageSize = Number(searchParams.get("pageSize") ?? 10);

  const [createSession, { isLoading: isCreating }] = useCreateSessionMutation();

  async function handleCreateSession() {
    if (isNaN(id)) return;

    try {
      await createSession({ quizId: id }).unwrap();
    } catch (e) {
      console.error(e);
    }
  }

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <GreenButton className="w-30 h-10" onClick={() => navigate(-1)}>
          Back
        </GreenButton>
        <h1 className="text-xl">Sessions</h1>

        <GreenButton
          className="w-30 h-10"
          onClick={handleCreateSession}
          disabled={isCreating}
        >
          Session Create
        </GreenButton>
      </div>
      <QuizSessionsListContainer
        page={page}
        pageSize={pageSize}
        onPageChange={setSearchParams}
      />
    </div>
  );
}
