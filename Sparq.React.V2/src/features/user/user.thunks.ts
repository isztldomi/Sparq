import { createAsyncThunk } from "@reduxjs/toolkit";
import { getProfileApi, updateNickNameApi } from "@/api/services/userService";
import { mapUser } from "@/features/user/user.mapper";

import { normalizeError } from "@/api/errors/handleAxiosError";

import type { NickNameUpdateRequestDto } from "@/features/user/user.types";

export const nickNameUpdate = createAsyncThunk(
  "users/nickname",
  async (data: NickNameUpdateRequestDto, { rejectWithValue }) => {
    try {
      return await updateNickNameApi(data);
    } catch (e) {
      return rejectWithValue(normalizeError(e));
    }
  },
);

export const fetchProfile = createAsyncThunk(
  "auth/fetchUser",
  async (_, { rejectWithValue }) => {
    try {
      const dto = await getProfileApi();
      return mapUser(dto);
    } catch (e) {
      return rejectWithValue(normalizeError(e));
    }
  },
);
