import { post } from "../http/http";
import type {
  LoginRequestDto,
  LoginResponseDto,
  RegisterRequestDto,
} from "@/features/auth/authTypes";
import type { UserResponseDto } from "@/features/user/userTypes";

export function loginApi(data: LoginRequestDto): Promise<LoginResponseDto> {
  return post("/users/login", data);
}

export function refreshTokenApi(
  refreshToken: string,
): Promise<LoginResponseDto> {
  return post("/users/refresh", {
    refreshToken,
  });
}

export function registerApi(
  data: RegisterRequestDto,
): Promise<UserResponseDto> {
  return post("/users", data);
}
