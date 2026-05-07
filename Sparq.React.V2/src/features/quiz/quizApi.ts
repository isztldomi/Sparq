import { baseApi } from "@/features/base/baseApi";
import {
  createQuizApi,
  deactivateQuizByIdApi,
  getMyQuizzesApi,
  getQuizByIdApi,
} from "@/api/services/quizService";
import { toApiError } from "@/api/core/toApiError";

export const quizApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    createQuiz: builder.mutation<any, any>({
      async queryFn(payload) {
        try {
          return { data: await createQuizApi(payload) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      invalidatesTags: [{ type: "Quiz", id: "LIST" }],
    }),

    getMyQuizzes: builder.query({
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
              { type: "Quiz", id: "LIST" },
              ...result.items.map((q) => ({ type: "Quiz", id: q.id })),
            ]
          : [{ type: "Quiz", id: "LIST" }],
    }),

    getQuizById: builder.query({
      async queryFn(id) {
        try {
          return { data: await getQuizByIdApi(id) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (_r, _e, id) => [{ type: "Quiz", id }],
    }),

    deactivateQuiz: builder.mutation({
      async queryFn(id) {
        try {
          await deactivateQuizByIdApi(id);
          return { data: undefined };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      invalidatesTags: (_r, _e, id) => [
        { type: "Quiz", id },
        { type: "Quiz", id: "LIST" },
      ],
    }),
  }),
});

export const {
  useCreateQuizMutation,
  useGetMyQuizzesQuery,
  useGetQuizByIdQuery,
  useDeactivateQuizMutation,
} = quizApi;
