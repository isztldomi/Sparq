export type AuthState = {
  token: string | null;
  refreshToken: string | null;
  loading: boolean;
};

export type LoginRequestDto = {
  email: string;
  password: string;
};

export type LoginResponseDto = {
  userId: string;
  authToken: string;
  refreshToken: string;
};

export type RegisterRequestDto = {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;
  password: string;
};

export type AuthData = {
  token: string;
  refreshToken: string;
};
