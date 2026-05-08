import { post } from "../http/http";
import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
} from "@/features/session/sessionTypes";

export function createSessionApi(
  data: CreateSessionRequestDto,
): Promise<CreatedSessionResponseDto> {
  return post("/session", data);
}
