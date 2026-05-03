import { post } from "../http/http";
import type {
  UserResponseDto,
  LoginRequest,
  LoginResponseDto,
  RegisterRequest,
} from "@/features/auth/auth.types";

export function loginApi(data: LoginRequest): Promise<LoginResponseDto> {
  return post("/users/login", data);
}

export function registerApi(data: RegisterRequest): Promise<UserResponseDto> {
  return post("/users", data);
}
