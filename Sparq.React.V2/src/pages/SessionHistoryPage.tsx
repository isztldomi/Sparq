import { useEffect } from "react";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { useGetSessionStatusByIdQuery } from "@/features/session/sessionApi";
import { SessionStatus } from "@/features/session/sessionTypes";
import { useNavigate, useParams } from "react-router-dom";
import { SessionHistoryContainer } from "@/components/containers/SessionHistoryContainer";

export function SessionHistoryPage() {
  const { sessionId } = useParams();

  const navigate = useNavigate();

  const {
    data: statusData,
    isLoading: isStatusLoading,
    isError: isStatusError,
  } = useGetSessionStatusByIdQuery({
    sessionId: sessionId!,
  });

  useEffect(() => {
    if (!statusData) return;

    switch (statusData.status) {
      case SessionStatus.Created:
        console.log("CASE: CREATED");
        // Ilyennek nem kéne lennie
        return;

      case SessionStatus.Waiting:
        console.log("CASE: WAITING");
        navigate(`/session/${sessionId}`);
        return;

      case SessionStatus.Running:
        console.log("CASE: RUNNING");
        navigate(`/session/${sessionId}`);
        return;

      case SessionStatus.Finished:
        console.log("CASE: FINISHED");
        break;

      default:
        console.log("CASE: UNKNOWN");
        break;
    }
  }, [statusData, navigate, sessionId]);

  if (isStatusLoading) return <LoadingIndicator />;

  if (isStatusError || !statusData) {
    navigate(`/history`);
    return;
  }

  return (
    <div>
      <SessionHistoryContainer statusData={statusData} />
    </div>
  );
}
