import { useParams } from "react-router-dom";

export function SessionHistoryPage() {
  const { sessionId } = useParams();
  return (
    <div>
      <div>SessionHistoryPage - {sessionId}</div>
    </div>
  );
}
