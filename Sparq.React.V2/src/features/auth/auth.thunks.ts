import { createAsyncThunk } from "@reduxjs/toolkit";
import { getProfile, loginApi, registerApi } from "@/api/auth.api";
import { mapUser } from "@/features/auth/auth.mapper";
import type { LoginRequest, RegisterRequest } from "@/features/auth/auth.types";

export const fetchProfile = createAsyncThunk(
  "auth/fetchUser",
  async (_, { rejectWithValue }) => {
    try {
      const dto = await getProfile();
      return mapUser(dto);
    } catch (e) {
      return rejectWithValue("Failed to load user\n" + e);
    }
  },
);

export const login = createAsyncThunk(
  "auth/login",
  async (data: LoginRequest, { dispatch }) => {
    const res = await loginApi(data);

    // tokeneket visszaadjuk
    const payload = {
      token: res.authToken,
      refreshToken: res.refreshToken,
      userId: res.userId,
    };

    // profile betöltése login után
    dispatch(fetchProfile());

    return payload;
  },
);

export const register = createAsyncThunk(
  "auth/register",
  async (data: RegisterRequest, { rejectWithValue }) => {
    try {
      const res = await registerApi(data);
      return res;
    } catch (e) {
      return rejectWithValue("Registration failed\n" + e);
    }
  },
);
