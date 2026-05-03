import type { RootState } from "@/app/store";

export const selectAuthLoading = (state: RootState) => state.auth.loading;

export const selectIsAuthenticated = (state: RootState) => !!state.auth.token;
