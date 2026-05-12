import { useState } from "react";

import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { useGetSessionPublicDataByIdQuery } from "@/features/session/sessionApi";
import { useStartSessionMutation } from "@/features/session/sessionApi";

import { GreenRedCheckbox } from "../checkbox/greenRedCheckbox";
import { SessionStatus } from "@/features/session/sessionTypes";
import { SessionStatusLabel } from "../label/SessionStatusLabel";
import { GreenButton } from "../buttons/greenButton";

import { useSessionManageContext } from "@/realtime/sessions/context/useSessionManageContext";

export function SessionManageMetaDetailsContainer() {
  const { sessionId, sessionData } = useSessionManageContext();

  const [startSession, { isLoading: isStartSessionLoading }] =
    useStartSessionMutation();

  const {
    data: publicSessionData,
    isLoading: isPublicLoading,
    isError: isPublicError,
  } = useGetSessionPublicDataByIdQuery(sessionId);

  const [isIdHidden, setIsIdHidden] = useState(true);
  const [isPinHidden, setIsPinHidden] = useState(true);

  if (isPublicLoading || isStartSessionLoading) {
    return <LoadingIndicator />;
  }

  if (isPublicError || !publicSessionData) {
    return null;
  }

  return (
    <div className="w-full rounded-lg bg-[var(--surface-4)] p-5 flex flex-col gap-4">
      <div className="flex items-center gap-4">
        Session id:
        <div className={isIdHidden ? "blur-sm" : ""}>{sessionData.id}</div>
        <GreenRedCheckbox value={isIdHidden} onChange={setIsIdHidden} />
      </div>

      <div className="flex items-center gap-4">
        Pin:
        <div className={isPinHidden ? "blur-sm" : ""}>
          {sessionData.pinCode}
        </div>
        <GreenRedCheckbox value={isPinHidden} onChange={setIsPinHidden} />
      </div>

      <div>Title: {publicSessionData.snapshot.title}</div>

      <div>Description: {publicSessionData.snapshot.description}</div>

      <div>
        Status:
        {sessionData.status === SessionStatus.Waiting && (
          <SessionStatusLabel variant="warning">Waiting</SessionStatusLabel>
        )}
        {sessionData.status === SessionStatus.Running && (
          <SessionStatusLabel variant="error">Running</SessionStatusLabel>
        )}
      </div>

      {sessionData.status === SessionStatus.Waiting && (
        <GreenButton onClick={() => startSession(sessionData.id)}>
          Start
        </GreenButton>
      )}
    </div>
  );
}
