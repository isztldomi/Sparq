import { useParams } from "react-router-dom";

import { SessionManageProvider } from "@/realtime/sessions/providers/SessionManageProvider";

import { SessionRealtimeProvider } from "@/realtime/sessions/providers/SessionRealtimeProvider";

import { SessionManagePageContent } from "./SessionManagePageContent";
import { useGetSessionStatusByIdQuery } from "@/features/session/sessionApi";

export function SessionManagePage() {
  const { sessionId } = useParams();

  const { data, isLoading, isError } = useGetSessionStatusByIdQuery({
    sessionId: sessionId!,
  });

  return (
    <SessionManageProvider sessionId={sessionId!}>
      <SessionRealtimeProvider sessionId={sessionId!}>
        <SessionManagePageContent sessionId={sessionId!} data={data} />
      </SessionRealtimeProvider>
    </SessionManageProvider>
  );
}
