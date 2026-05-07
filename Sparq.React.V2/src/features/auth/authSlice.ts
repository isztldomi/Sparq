import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { AuthState } from "@/features/auth/authTypes";

const initialState: AuthState = {
  token: null,
  refreshToken: null,
  loading: false, // opcionális, akár ki is veheted később
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setAuth(
      state,
      action: PayloadAction<{ token: string; refreshToken: string }>,
    ) {
      state.token = action.payload.token;
      state.refreshToken = action.payload.refreshToken;
    },

    logout(state) {
      state.token = null;
      state.refreshToken = null;

      localStorage.removeItem("auth");
    },
  },
});

export const { logout, setAuth } = authSlice.actions;
export default authSlice.reducer;
