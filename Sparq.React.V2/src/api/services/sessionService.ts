import type { PagedResult } from "@/features/page/pageTypes";
import { post, patch, get } from "../http/http";
import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
  JoinSessionExtUserResponseDto,
  JoinSessionRequestDto,
  SessionPublicWaitingListDto,
} from "@/features/session/sessionTypes";

export function createSessionApi(
  data: CreateSessionRequestDto,
): Promise<CreatedSessionResponseDto> {
  return post("/session", data);
}

export function activateForWaitingSessionApi(sessionId: string): Promise<void> {
  return patch(`/session/${sessionId}/activate-waiting`);
}

export function getAllPublicWaitingSessionsApi(
  page: number,
  pageSize: number,
): Promise<PagedResult<SessionPublicWaitingListDto>> {
  return get(`/session/public-waiting?page=${page}&pageSize=${pageSize}`);
}

export function getSessionByIdApi(
  sessionId: string,
): Promise<CreatedSessionResponseDto> {
  return get(`/session/${sessionId}`);
}

export function getSessionPublicDataByIdApi(
  sessionId: string,
): Promise<SessionPublicWaitingListDto> {
  return get(`/session/${sessionId}/public`);
}

export function joinSessionApi(data: JoinSessionRequestDto): Promise<void> {
  return post("/session/join", data);
}

export function extUserJoinSessionApi(
  data: JoinSessionRequestDto,
): Promise<JoinSessionExtUserResponseDto> {
  return post("/session/ext-user-join", data);
}
