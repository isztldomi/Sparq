import { createSlice } from "@reduxjs/toolkit";
import type { UserStateDto } from "@/features/user/user.types";
import { fetchProfile, nickNameUpdate } from "@/features/user/user.thunks";

const initialState: UserStateDto = {
  user: null,
  loading: false,
};

const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    resetUser: (state) => {
      state.user = null;
      state.loading = false;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchProfile.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchProfile.fulfilled, (state, action) => {
        state.user = action.payload;
      })
      .addCase(fetchProfile.rejected, (state) => {
        state.user = null;
      })

      .addCase(nickNameUpdate.pending, (state) => {
        state.loading = true;
      })
      .addCase(nickNameUpdate.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
      })
      .addCase(nickNameUpdate.rejected, (state) => {
        state.loading = false;
      });
  },
});

export const { resetUser } = userSlice.actions;
export default userSlice.reducer;
