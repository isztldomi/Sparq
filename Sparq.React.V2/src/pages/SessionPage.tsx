import { useParams } from "react-router-dom";
import { useSessionConnection } from "@/features/session/useSessionConnection";

export function SessionPage() {
  const { sessionId } = useParams();

  useSessionConnection(sessionId);

  return <div>sessionId: {sessionId}</div>;
}
