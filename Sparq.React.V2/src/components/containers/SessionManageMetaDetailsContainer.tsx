import { useState } from "react";

import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import {
  useGetSessionByIdQuery,
  useGetSessionPublicDataByIdQuery,
} from "@/features/session/sessionApi";
import { useNavigate } from "react-router-dom";
import { GreenRedCheckbox } from "../checkbox/greenRedCheckbox";
import { SessionStatus } from "@/features/session/sessionTypes";
import { SessionStatusLabel } from "../label/SessionStatusLabel";

type Props = {
  sessionId: string;
};

export function SessionManageMetaDetailsContainer({ sessionId }: Props) {
  const navigate = useNavigate();

  const [isBlurred, setIsBlurred] = useState(true);

  const {
    data: sessionData,
    isLoading: isSessionLoading,
    isError: isSessionError,
  } = useGetSessionByIdQuery(sessionId);

  const {
    data: publicSessionData,
    isLoading: isPublicSessionLoading,
    isError: isPublicSessionError,
  } = useGetSessionPublicDataByIdQuery(sessionId);

  if (isSessionLoading || isPublicSessionLoading) {
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
          Snapshot id:
          <div
            className={`bg-[var(--surface-5)] p-4 rounded-lg transition-all text-[var(--error-text)] duration-200 ${
              isBlurred ? "blur-sm select-none" : ""
            }`}
          >
            {sessionData.id}
          </div>
        </div>

        <GreenRedCheckbox
          value={isBlurred}
          onChange={setIsBlurred}
          trueLabel="Hide"
          falseLabel="Show"
          className="rounded-lg text-sm w-20 h-10"
        />
      </div>
      <div className="flex flex-wrap whitespace-pre-wrap">
        Title: <p>{publicSessionData.snapshot.title}</p>
      </div>
      <div className="flex flex-wrap whitespace-pre-wrap">
        Description: <p>{publicSessionData.snapshot.description}</p>
      </div>
      <div className="flex flex-wrap whitespace-pre-wrap gap-2">
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
      </div>
    </div>
  );
}
