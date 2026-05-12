import { useParams } from "react-router-dom";

import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

import { useGetSessionStatusByIdQuery } from "@/features/session/sessionApi";

import { SessionRealtimeProvider } from "@/realtime/sessions/providers/SessionRealtimeProvider";

import { ParticipantPageContent } from "@/components/containers/ParticipantPageContent";

export function SessionParticipantPage() {
  const { sessionId } = useParams();

  const extUserId = sessionId
    ? (localStorage.getItem(sessionId) ?? undefined)
    : undefined;

  const { data, isLoading, isError } = useGetSessionStatusByIdQuery({
    sessionId: sessionId!,
    extUserId,
  });

  if (isLoading) return <LoadingIndicator />;
  if (isError || !data) return <div>Something went wrong.</div>;

  return (
    <SessionRealtimeProvider sessionId={sessionId!}>
      <ParticipantPageContent
        sessionId={sessionId!}
        extUserId={extUserId}
        data={data}
      />
    </SessionRealtimeProvider>
  );
}
