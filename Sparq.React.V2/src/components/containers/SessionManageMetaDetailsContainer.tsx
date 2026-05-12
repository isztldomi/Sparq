import { useState } from "react";

import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import {
  useGetSessionByIdQuery,
  useGetSessionPublicDataByIdQuery,
  useStartSessionMutation,
} from "@/features/session/sessionApi";
import { useNavigate } from "react-router-dom";
import { GreenRedCheckbox } from "../checkbox/greenRedCheckbox";
import { SessionStatus } from "@/features/session/sessionTypes";
import { SessionStatusLabel } from "../label/SessionStatusLabel";
import { GreenButton } from "../buttons/greenButton";

type SessionManageMetaDetailsContainerProps = {
  sessionId: string;
  sessionData?: ReturnType<typeof useGetSessionByIdQuery>["data"];
  isSessionLoading?: boolean;
  isSessionError?: boolean;
};

export function SessionManageMetaDetailsContainer({
  sessionId,
  sessionData,
  isSessionLoading,
  isSessionError,
}: SessionManageMetaDetailsContainerProps) {
  const navigate = useNavigate();

  const [isSessionIdBlurred, setIsSessionIdBlurred] = useState(true);
  const [isSessionPinCodeBlurred, setIsSessionPinCodeBlurred] = useState(true);

  const [startSession, { isLoading: isStartSessionLoading }] =
    useStartSessionMutation();

  const {
    data: publicSessionData,
    isLoading: isPublicSessionLoading,
    isError: isPublicSessionError,
  } = useGetSessionPublicDataByIdQuery(sessionId);

  if (isSessionLoading || isPublicSessionLoading || isStartSessionLoading) {
    return <LoadingIndicator />;
  }

  if (
    isSessionError ||
    !sessionData ||
    isPublicSessionError ||
    !publicSessionData
  ) {
    navigate("/sessions/notFound", { replace: true });
    return null;
  }

  return (
    <div className="w-full rounded-lg bg-[var(--surface-4)] p-5 flex flex-col gap-4">
      <div className="flex flex-wrap items-center gap-4">
        <div className="flex flex-wrap items-center gap-2">
          Session id:
          <div
            className={`bg-[var(--surface-5)] p-4 rounded-lg transition-all text-[var(--error-text)] duration-200 ${
              isSessionIdBlurred ? "blur-sm select-none" : ""
            }`}
          >
            {sessionData.id}
          </div>
        </div>

        <GreenRedCheckbox
          value={isSessionIdBlurred}
          onChange={setIsSessionIdBlurred}
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
              isSessionPinCodeBlurred ? "blur-sm select-none" : ""
            }`}
          >
            {sessionData.pinCode}
          </div>
        </div>

        <GreenRedCheckbox
          value={isSessionPinCodeBlurred}
          onChange={setIsSessionPinCodeBlurred}
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
