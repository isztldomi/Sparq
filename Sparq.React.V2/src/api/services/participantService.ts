import type {
  ParticipantIsJoinedResponseDto,
  ParticipantPublicListResponseDto,
} from "@/features/participant/participantTypes";
import { get, del } from "../http/http";
import { buildQuery } from "@/api/core/queryString";

export function isJoinedApi(
  sessionId: string,
  extUserId?: string,
): Promise<ParticipantIsJoinedResponseDto> {
  return get(`participant/${sessionId}/is-joined${buildQuery({ extUserId })}`);
}

export function getParticipantsBySessionIdApi(
  sessionId: string,
  extUserId?: string,
): Promise<ParticipantPublicListResponseDto[]> {
  return get(
    `participant/${sessionId}/participants${buildQuery({ extUserId })}`,
  );
}

export function deleteParticipantFromSessionByIdApi(
  sessionId: string,
  participantId: string,
): Promise<boolean> {
  return del(`participant/${sessionId}/${participantId}`);
}
