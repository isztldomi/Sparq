import { createApi } from "@reduxjs/toolkit/query/react";
import { createQuizApi } from "@/api/services/quizService";
import { toApiError } from "@/api/core/toApiError";

export const quizApi = createApi({
  reducerPath: "quizApi",
  baseQuery: async () => ({ data: {} }),

  tagTypes: ["Quiz"],

  endpoints: (builder) => ({
    createQuiz: builder.mutation<any, any>({
      async queryFn(payload) {
        try {
          const dto = await createQuizApi(payload);
          return { data: dto };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      invalidatesTags: ["Quiz"],
    }),
  }),
});

export const { useCreateQuizMutation } = quizApi;
