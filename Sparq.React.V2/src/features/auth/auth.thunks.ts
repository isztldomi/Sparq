import { createAsyncThunk } from "@reduxjs/toolkit";
import { loginApi, registerApi } from "@/api/services/authService";
import { getProfileApi } from "@/api/services/userService";
import { mapUser } from "@/features/auth/auth.mapper";
import { normalizeError } from "@/api/errors/handleAxiosError";
import type { LoginRequest, RegisterRequest } from "@/features/auth/auth.types";

export const fetchProfile = createAsyncThunk(
  "auth/fetchUser",
  async (_, { rejectWithValue }) => {
    try {
      const dto = await getProfileApi();
      return mapUser(dto);
    } catch (e) {
      return rejectWithValue(e);
    }
  },
);

export const login = createAsyncThunk(
  "auth/login",
  async (data: LoginRequest, { dispatch, rejectWithValue }) => {
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
      return rejectWithValue(e);
    }
  },
);

export const register = createAsyncThunk(
  "auth/register",
  async (data: RegisterRequest, { rejectWithValue }) => {
    try {
      return await registerApi(data);
    } catch (e) {
      return rejectWithValue(normalizeError(e));
    }
  },
);
