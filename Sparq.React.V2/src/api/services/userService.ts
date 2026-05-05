import type { NickNameUpdateRequestDto } from "@/features/user/userTypes";
import { get, patch } from "../http/http";
import type { UserResponseDto } from "@/features/user/userTypes";

export function getProfileApi(): Promise<UserResponseDto> {
  return get("/users");
}

export function updateNickNameApi(
  data: NickNameUpdateRequestDto,
): Promise<UserResponseDto> {
  return patch("/users/nickname", data);
}
