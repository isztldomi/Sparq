import axios from "axios";
import type {
  UserResponseDto,
  LoginRequest,
  LoginResponseDto,
  RegisterRequest,
} from "@/features/auth/auth.types";

export const getProfile = async (): Promise<UserResponseDto> => {
  const res = await axios.get("/api/users");
  return res.data;
};

export const loginApi = async (
  data: LoginRequest,
): Promise<LoginResponseDto> => {
  const res = await axios.post("/api/users/login", data);
  return res.data;
};

export const registerApi = async (
  data: RegisterRequest,
): Promise<UserResponseDto> => {
  const res = await axios.post("/api/users", data);
  return res.data;
};
