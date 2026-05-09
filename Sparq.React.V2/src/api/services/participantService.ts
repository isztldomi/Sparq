import type { ParticipantIsJoinedResponseDto } from "@/features/participant/participantTypes";
import { get } from "../http/http";

export function isJoinedApi(
  sessionId: string,
): Promise<ParticipantIsJoinedResponseDto> {
  return get(`participant/${sessionId}/is-joined`);
}

export function extUserIsJoinedApi(
  sessionId: string,
  extUserId: string,
): Promise<ParticipantIsJoinedResponseDto> {
  return get(`participant/${sessionId}/ext-user-is-joined/${extUserId}`);
}
