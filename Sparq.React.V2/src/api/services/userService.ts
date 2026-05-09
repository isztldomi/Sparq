import type {
  NickNameUpdateRequestDto,
  UserResponseDto,
} from "@/features/user/userTypes";
import { get, patch } from "../http/http";

export function getProfileApi(): Promise<UserResponseDto> {
  return get("/users");
}

export function updateNickNameApi(
  data: NickNameUpdateRequestDto,
): Promise<UserResponseDto> {
  return patch("/users/nickname", data);
}

export function getCurrentUserApi(): Promise<UserResponseDto | null> {
  return get("/users/current");
}
