import { baseApi } from "@/features/base/baseApi";
import {
  createQuizApi,
  deactivateQuizByIdApi,
  getMyQuizzesApi,
  getQuizByIdApi,
  getQuizSessionsByIdApi,
} from "@/api/services/quizService";
import { toApiError } from "@/api/core/toApiError";
import type { PagedResult } from "../page/pageTypes";
import type {
  MyQuizListDto,
  QuizCreateRequestDto,
  QuizResponseDto,
} from "./quizTypes";
import type { MyQuizSessionsListDto } from "../session/sessionTypes";

export const quizApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    createQuiz: builder.mutation<QuizResponseDto, QuizCreateRequestDto>({
      async queryFn(payload) {
        try {
          return { data: await createQuizApi(payload) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      invalidatesTags: [{ type: "Quiz", id: "LIST" }],
    }),

    getMyQuizzes: builder.query<
      PagedResult<MyQuizListDto>,
      { page: number; pageSize: number }
    >({
      async queryFn({ page, pageSize }) {
        try {
          return { data: await getMyQuizzesApi(page, pageSize) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (result) =>
        result
          ? [
              { type: "Quiz" as const, id: "LIST" },
              ...result.items.map((q) => ({
                type: "Quiz" as const,
                id: q.id,
              })),
            ]
          : [{ type: "Quiz" as const, id: "LIST" }],
    }),

    getQuizById: builder.query<QuizResponseDto, number>({
      async queryFn(id) {
        try {
          return { data: await getQuizByIdApi(id) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (_r, _e, id) => [{ type: "Quiz", id }],
    }),

    deactivateQuiz: builder.mutation<null, number>({
      async queryFn(id) {
        try {
          await deactivateQuizByIdApi(id);
          return { data: null };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      invalidatesTags: (_r, _e, id) => [
        { type: "Quiz", id },
        { type: "Quiz", id: "LIST" },
      ],
    }),

    getQuizSessionsById: builder.query<
      PagedResult<MyQuizSessionsListDto>,
      { id: number; page: number; pageSize: number }
    >({
      async queryFn({ id, page, pageSize }) {
        try {
          return {
            data: await getQuizSessionsByIdApi(id, page, pageSize),
          };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (result, _error, payload) =>
        result
          ? [
              { type: "Session" as const, id: payload.id },
              { type: "Session" as const, id: "LIST" },
            ]
          : [{ type: "Session" as const, id: "LIST" }],
    }),
  }),
});

export const {
  useCreateQuizMutation,
  useGetMyQuizzesQuery,
  useGetQuizByIdQuery,
  useDeactivateQuizMutation,
  useGetQuizSessionsByIdQuery,
} = quizApi;
