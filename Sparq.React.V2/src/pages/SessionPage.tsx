import { useParams } from "react-router-dom";

export function SessionPage() {
  const { sessionId } = useParams();

  return <div>sessionId: {sessionId}</div>;
}
