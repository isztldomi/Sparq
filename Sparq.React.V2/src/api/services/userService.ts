import type { NickNameUpdateRequestDto } from "@/features/user/user.types";
import { get, patch } from "../http/http";
import type { UserResponseDto } from "@/features/user/user.types";

export function getProfileApi(): Promise<UserResponseDto> {
  return get("/users");
}

export function updateNickNameApi(
  data: NickNameUpdateRequestDto,
): Promise<UserResponseDto> {
  return patch("/users/nickname", data);
}
