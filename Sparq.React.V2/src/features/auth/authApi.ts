import { baseApi } from "@/features/base/baseApi";
import { loginApi, registerApi } from "@/api/services/authService";
import { toApiError } from "@/api/core/toApiError";
import type {
  LoginRequestDto,
  LoginResponseDto,
  RegisterRequestDto,
} from "@/features/auth/authTypes";
import type { UserResponseDto } from "../user/userTypes";

export const authApi = baseApi.injectEndpoints({
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
