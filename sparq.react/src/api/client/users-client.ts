import { get, postAsJson, postAsJsonWithoutResponse } from "@/api/client/http";
import type { LoginResponseDto } from "@/api/models/loginDto/LoginResponseDto";
import type { LoginRequestDto } from "@/api/models/loginDto/LoginRequestDto";
import type { UserRequestDto } from "@/api/models/userDto/UserRequestDto";
import type { UserResponseDto } from "@/api/models/userDto/UserResponseDto";
import type { RedeemRefreshTokenRequestDto } from "@/api/models/tokenDto/RedeemRefreshTokenRequestDto";

export async function login(
  loginDto: LoginRequestDto,
): Promise<LoginResponseDto> {
  return await postAsJson<LoginRequestDto, LoginResponseDto>(
    "users/login",
    loginDto,
  );
}

export async function logout(): Promise<void> {
  await postAsJsonWithoutResponse("users/logout");
}

export async function refresh(
  redeemRefreshTokenRequestDto: RedeemRefreshTokenRequestDto,
): Promise<LoginResponseDto> {
  return await postAsJson<RedeemRefreshTokenRequestDto, LoginResponseDto>(
    "users/refresh",
    redeemRefreshTokenRequestDto,
  );
}

export async function createUser(
  data: UserRequestDto,
): Promise<UserResponseDto> {
  return await postAsJson<UserRequestDto, UserResponseDto>("users", data);
}

export async function getUserById(id: string): Promise<UserResponseDto> {
  return get<UserResponseDto>(`users/${id}`);
}
