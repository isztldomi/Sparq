import { GreenButton } from "@/components/buttons/greenButton";

import { SessionManageMetaDetailsContainer } from "@/components/containers/SessionManageMetaDetailsContainer";

import { SessionManageParticipantsContainer } from "@/components/containers/SessionManageParticipantsContainer";
import { SessionManageQuestionContainer } from "@/components/containers/SessionManageQuestionContainer";

import { useNavigate } from "react-router-dom";

interface SessionManagePageContentProps {
  sessionId: string;
  data: any;
}

export function SessionManagePageContent({
  sessionId,
  data,
}: SessionManagePageContentProps) {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Session Manage</h1>

        <GreenButton className="w-30 h-10" onClick={() => navigate(-1)}>
          Back
        </GreenButton>
      </div>

      <div className="pt-4">
        <SessionManageMetaDetailsContainer />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 mt-4">
        <SessionManageParticipantsContainer />

        <SessionManageQuestionContainer sessionId={sessionId} />
      </div>
    </div>
  );
}
