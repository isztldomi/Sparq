import { configureStore } from "@reduxjs/toolkit";
import { authApi } from "@/features/auth/authApi";
import { userApi } from "@/features/user/userApi";
import { mediaApi } from "@/features/media/mediaApi";
import { quizApi } from "@/features/quiz/quizApi";

export const store = configureStore({
  reducer: {
    [authApi.reducerPath]: authApi.reducer,
    [userApi.reducerPath]: userApi.reducer,
    [mediaApi.reducerPath]: mediaApi.reducer,
    [quizApi.reducerPath]: quizApi.reducer,
  },

  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(
      authApi.middleware,
      userApi.middleware,
      mediaApi.middleware,
      quizApi.middleware,
    ),
});
