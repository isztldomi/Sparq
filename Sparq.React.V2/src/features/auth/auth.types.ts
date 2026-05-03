import type { User } from "@/shared/types/user";

export type AuthState = {
  user: User | null;
  token: string | null;
  refreshToken: string | null;
  loading: boolean;
};

export type UserResponseDto = {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;
};

export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponseDto = {
  userId: string;
  authToken: string;
  refreshToken: string;
};

export type RegisterRequest = {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;
  password: string;
};
