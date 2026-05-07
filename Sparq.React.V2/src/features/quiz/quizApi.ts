import { createApi } from "@reduxjs/toolkit/query/react";
import {
  createQuizApi,
  getMyQuizzesApi,
  getQuizByIdApi,
} from "@/api/services/quizService";
import { toApiError } from "@/api/core/toApiError";
import type { PagedResult } from "../page/pageTypes";
import type { MyQuizListDto, QuizResponseDto } from "./quizTypes";

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
    getMyQuizzes: builder.query<
      PagedResult<MyQuizListDto>,
      { page: number; pageSize: number }
    >({
      async queryFn({ page, pageSize }) {
        try {
          const dto = await getMyQuizzesApi(page, pageSize);
          return { data: dto };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: ["Quiz"],
    }),
    getQuizByIdApi: builder.query<QuizResponseDto, number>({
      async queryFn(id) {
        try {
          const dto = await getQuizByIdApi(id);

          return { data: dto };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (_result, _error, id) => [{ type: "Quiz", id }],
    }),
  }),
});

export const {
  useCreateQuizMutation,
  useGetMyQuizzesQuery,
  useGetQuizByIdApiQuery,
} = quizApi;
