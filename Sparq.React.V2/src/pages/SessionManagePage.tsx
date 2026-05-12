import { useParams } from "react-router-dom";

import { SessionManageProvider } from "@/realtime/sessions/providers/SessionManageProvider";

import { SessionRealtimeProvider } from "@/realtime/sessions/providers/SessionRealtimeProvider";

import { SessionManagePageContent } from "./SessionManagePageContent";

export function SessionManagePage() {
  const { sessionId } = useParams();

  return (
    <SessionManageProvider sessionId={sessionId!}>
      <SessionRealtimeProvider sessionId={sessionId!}>
        <SessionManagePageContent />
      </SessionRealtimeProvider>
    </SessionManageProvider>
  );
}
