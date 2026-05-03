import { post } from "../http/http";
import type {
  LoginRequestDto,
  LoginResponseDto,
  RegisterRequestDto,
} from "@/features/auth/auth.types";
import type { UserResponseDto } from "@/features/user/user.types";

export function loginApi(data: LoginRequestDto): Promise<LoginResponseDto> {
  return post("/users/login", data);
}

export function registerApi(
  data: RegisterRequestDto,
): Promise<UserResponseDto> {
  return post("/users", data);
}
