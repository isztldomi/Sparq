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
      <div className="flex flex-wrap items-center gap-4">
        <div className="flex flex-wrap items-center gap-2">
          Session id:
          <div
            className={`bg-[var(--surface-5)] p-4 rounded-lg transition-all text-[var(--error-text)] duration-200 ${
              isIdHidden ? "blur-sm select-none" : ""
            }`}
          >
            {sessionData.id}
          </div>
        </div>

        <GreenRedCheckbox
          value={isIdHidden}
          onChange={setIsIdHidden}
          trueLabel="Show"
          falseLabel="Hide"
          className="rounded-lg text-sm w-20 h-10"
        />
      </div>
      <div className="flex flex-wrap items-center gap-4">
        <div className="flex flex-wrap items-center gap-2">
          Session PinCode:
          <div
            className={`bg-[var(--surface-5)] p-4 rounded-lg transition-all text-[var(--error-text)] duration-200 ${
              isPinHidden ? "blur-sm select-none" : ""
            }`}
          >
            {sessionData.pinCode}
          </div>
        </div>

        <GreenRedCheckbox
          value={isPinHidden}
          onChange={setIsPinHidden}
          trueLabel="Show"
          falseLabel="Hide"
          className="rounded-lg text-sm w-20 h-10"
        />
      </div>

      <div className="flex flex-wrap whitespace-pre-wrap">
        Title: <p>{publicSessionData.snapshot.title}</p>
      </div>
      <div className="flex flex-wrap whitespace-pre-wrap">
        Description: <p>{publicSessionData.snapshot.description}</p>
      </div>

      <div className="flex flex-wrap items-center whitespace-pre-wrap gap-2">
        Status:
        <div>
          {sessionData.status === SessionStatus.Created && (
            <SessionStatusLabel variant="neutral">
              Not Started
            </SessionStatusLabel>
          )}

          {sessionData.status === SessionStatus.Waiting && (
            <SessionStatusLabel variant="warning">Waiting</SessionStatusLabel>
          )}

          {sessionData.status === SessionStatus.Running && (
            <SessionStatusLabel variant="error">Running</SessionStatusLabel>
          )}

          {sessionData.status === SessionStatus.Finished && (
            <SessionStatusLabel variant="info">Ended</SessionStatusLabel>
          )}
        </div>
        <div>
          {sessionData.status === SessionStatus.Waiting && (
            <GreenButton
              className="w-20 h-10"
              onClick={() => startSession(sessionData.id)}
            >
              Start
            </GreenButton>
          )}
        </div>
      </div>
    </div>
  );
}
