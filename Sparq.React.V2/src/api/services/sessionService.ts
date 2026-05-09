import { post, patch } from "../http/http";
import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
} from "@/features/session/sessionTypes";

export function createSessionApi(
  data: CreateSessionRequestDto,
): Promise<CreatedSessionResponseDto> {
  return post("/session", data);
}

export function activateForWaitingSessionApi(sessionId: string): Promise<void> {
  return patch(`/session/${sessionId}/activate-waiting`);
}
