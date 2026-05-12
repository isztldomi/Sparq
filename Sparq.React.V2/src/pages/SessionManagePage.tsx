import { GreenButton } from "@/components/buttons/greenButton";
import { useNavigate, useParams } from "react-router-dom";
import { SessionManageMetaDetailsContainer } from "@/components/containers/SessionManageMetaDetailsContainer";
import { SessionManageParticipantsContainer } from "@/components/containers/SessionManageParticipantsContainer";
import { useGetSessionByIdQuery } from "@/features/session/sessionApi";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

export function SessionManagePage() {
  const { sessionId } = useParams();
  const navigate = useNavigate();

  const {
    data: sessionData,
    isLoading: isSessionLoading,
    isError: isSessionError,
  } = useGetSessionByIdQuery(sessionId as string);

  if (isSessionLoading) {
    return <LoadingIndicator />;
  }

  if (isSessionError || !sessionData) {
    navigate("sessions/notFound");
  }

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Session Manage</h1>

        <GreenButton className="w-30 h-10" onClick={() => navigate(-1)}>
          Back
        </GreenButton>
      </div>
      <div className="pt-4">
        <SessionManageMetaDetailsContainer
          sessionId={sessionId!}
          sessionData={sessionData}
          isSessionLoading={isSessionLoading}
          isSessionError={isSessionError}
        />
      </div>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 mt-4">
        <SessionManageParticipantsContainer
          sessionId={sessionId!}
          sessionData={sessionData}
          isSessionLoading={isSessionLoading}
          isSessionError={isSessionError}
        />
        <div>asd</div>
      </div>
    </div>
  );
}
