import { configureStore } from "@reduxjs/toolkit";
import { authApi } from "@/features/auth/authApi";
import { userApi } from "@/features/user/userApi";
import { mediaApi } from "@/features/media/mediaApi";
import { quizApi } from "@/features/quiz/quizApi";
import { snapshotApi } from "@/features/snapshot/snapshotApi";

export const store = configureStore({
  reducer: {
    [authApi.reducerPath]: authApi.reducer,
    [userApi.reducerPath]: userApi.reducer,
    [mediaApi.reducerPath]: mediaApi.reducer,
    [quizApi.reducerPath]: quizApi.reducer,
    [snapshotApi.reducerPath]: snapshotApi.reducer,
  },

  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(
      authApi.middleware,
      userApi.middleware,
      mediaApi.middleware,
      quizApi.middleware,
      snapshotApi.middleware,
    ),
});
