import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { SessionStatus } from "@/features/session/sessionTypes";
import { useGetSessionStatusByIdQuery } from "@/features/session/sessionApi";
import { useParams } from "react-router-dom";
import { SessionWaitingContainer } from "@/components/containers/SessionWaitingContainer";

export function SessionParticipantPage() {
  const { sessionId } = useParams();

  const extUserId = sessionId
    ? (localStorage.getItem(sessionId) ?? undefined)
    : undefined;

  const { data, isLoading, isError } = useGetSessionStatusByIdQuery({
    sessionId: sessionId!,
    extUserId,
  });

  if (isLoading) {
    return <LoadingIndicator />;
  }

  if (isError || !data) {
    return <div>Something went wrong.</div>;
  }

  switch (data.status) {
    case SessionStatus.Created:
      return <div>This session is just created.</div>;

    case SessionStatus.Waiting:
      return (
        <SessionWaitingContainer sessionId={sessionId!} extUserId={extUserId} />
      );

    case SessionStatus.Running:
      return <div>SessionRunningPage</div>;

    case SessionStatus.Finished:
      return <div>SessionFinishedPage</div>;

    default:
      return <div>Unknown session status.</div>;
  }
}
