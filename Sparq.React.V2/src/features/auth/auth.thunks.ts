import { createAsyncThunk } from "@reduxjs/toolkit";
import { loginApi, registerApi } from "@/api/services/authService";
import { normalizeError } from "@/api/errors/handleAxiosError";
import type {
  LoginRequestDto,
  RegisterRequestDto,
} from "@/features/auth/auth.types";
import { fetchProfile } from "@/features/user/user.thunks";

export const login = createAsyncThunk(
  "auth/login",
  async (data: LoginRequestDto, { dispatch, rejectWithValue }) => {
    try {
      const res = await loginApi(data);

      const payload = {
        token: res.authToken,
        refreshToken: res.refreshToken,
      };

      localStorage.setItem("auth", JSON.stringify(payload));

      await dispatch(fetchProfile()).unwrap();

      return payload;
    } catch (e) {
      return rejectWithValue(normalizeError(e));
    }
  },
);

export const register = createAsyncThunk(
  "auth/register",
  async (data: RegisterRequestDto, { rejectWithValue }) => {
    try {
      return await registerApi(data);
    } catch (e) {
      return rejectWithValue(normalizeError(e));
    }
  },
);
