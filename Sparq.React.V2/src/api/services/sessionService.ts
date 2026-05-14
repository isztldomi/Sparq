import type { PagedResult } from "@/features/page/pageTypes";
import { post, patch, get, del } from "../http/http";
import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
  JoinSessionResponseDto,
  JoinSessionRequestDto,
  quitSessionRequestDto,
  SessionPublicWaitingListDto,
  SessionStatusResponseDto,
  SessionLeaderboardDto,
  MySessionListDto,
} from "@/features/session/sessionTypes";
import { buildQuery } from "../core/queryString";

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
  return get(`/session/public-waiting${buildQuery({ page, pageSize })}`);
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

export function joinSessionApi(
  data: JoinSessionRequestDto,
): Promise<JoinSessionResponseDto> {
  return post("/session/join", data);
}

export function getSessionStatusByIdApi(
  sessionId: string,
  extUserId?: string,
): Promise<SessionStatusResponseDto> {
  return get(`/session/${sessionId}/status${buildQuery({ extUserId })}`);
}

export function quitSessionApi(data: quitSessionRequestDto): Promise<boolean> {
  return post("/session/quit", data);
}

export function deleteSessionApi(sessionId: string): Promise<boolean> {
  return del(`/session/${sessionId}`);
}

export function deactivateSessionApi(sessionId: string): Promise<boolean> {
  return patch(`/session/${sessionId}/deactivate`);
}

export function startSessionApi(sessionId: string): Promise<boolean> {
  return patch(`/session/${sessionId}/start`);
}

export function nextQuestionSessionApi(sessionId: string): Promise<boolean> {
  return patch(`/session/${sessionId}/nextQuestion`);
}

export function leadboardSessionApi(
  sessionId: string,
  extUserId?: string,
): Promise<SessionLeaderboardDto> {
  return get(`/session/${sessionId}/leaderboard${buildQuery({ extUserId })}`);
}

export function historyApi(
  page: number,
  pageSize: number,
): Promise<PagedResult<MySessionListDto>> {
  return get(`/session/history${buildQuery({ page, pageSize })}`);
}
