import type { PagedResult } from "@/features/page/pageTypes";
import { post, patch, get } from "../http/http";
import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
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
