import { get } from "../http/http";
import type { UserResponseDto } from "@/features/auth/auth.types";

export function getProfileApi(): Promise<UserResponseDto> {
  return get("/users");
}
