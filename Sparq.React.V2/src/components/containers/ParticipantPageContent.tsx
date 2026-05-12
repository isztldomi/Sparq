import { SessionStatus } from "@/features/session/sessionTypes";
import { useSessionParticipantsUpdated } from "@/realtime/sessions/hooks/useSessionParticipantsUpdated";
import { SessionWaitingContainer } from "./SessionWaitingContainer";

export function ParticipantPageContent({ sessionId, extUserId, data }: any) {
  useSessionParticipantsUpdated();

  switch (data.status) {
    case SessionStatus.Created:
      return <div>This session is just created.</div>;

    case SessionStatus.Waiting:
      return (
        <SessionWaitingContainer sessionId={sessionId} extUserId={extUserId} />
      );

    case SessionStatus.Running:
      return <div>SessionRunningPage</div>;

    case SessionStatus.Finished:
      return <div>SessionFinishedPage</div>;

    default:
      return <div>Unknown session status.</div>;
  }
}
