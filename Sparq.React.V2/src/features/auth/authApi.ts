import { createApi } from "@reduxjs/toolkit/query/react";
import { loginApi, registerApi } from "@/api/services/authService";
import type {
  LoginRequestDto,
  LoginResponseDto,
  RegisterRequestDto,
} from "@/features/auth/authTypes";
import { toApiError } from "@/api/core/toApiError";
import type { UserResponseDto } from "../user/userTypes";

export const authApi = createApi({
  reducerPath: "authApi",
  baseQuery: async () => ({ data: {} }),

  endpoints: (builder) => ({
    login: builder.mutation<LoginResponseDto, LoginRequestDto>({
      async queryFn(data) {
        try {
          return { data: await loginApi(data) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
    }),

    register: builder.mutation<UserResponseDto, RegisterRequestDto>({
      async queryFn(data) {
        try {
          return { data: await registerApi(data) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
    }),
  }),
});

export const { useLoginMutation, useRegisterMutation } = authApi;
