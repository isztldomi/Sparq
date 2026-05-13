import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import type {
  SessionQuestionAnswersResponseDto,
  SubmitAnswerRequestDto,
} from "@/features/answer/answerTypes";
import {
  getSessionQuestionAnswersApi,
  submitAnswerApi,
} from "@/api/services/answerService";

export const answerApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    submitAnswer: builder.mutation<boolean, SubmitAnswerRequestDto>({
      async queryFn(arg) {
        try {
          const result = await submitAnswerApi(arg);
          return { data: result };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      invalidatesTags: (result, error, arg) => [
        {
          type: "SessionQuestionAnswers",
          id: `${arg.sessionId}-${arg.questionId}`,
        },
        { type: "Answer", id: "LIST" },
      ],
    }),
    getSessionQuestionAnswers: builder.query<
      SessionQuestionAnswersResponseDto,
      { sessionId: string; questionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, questionId, extUserId }) {
        try {
          const result = await getSessionQuestionAnswersApi(
            sessionId,
            questionId,
            extUserId,
          );
          return { data: result };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: (result, error, arg) => [
        {
          type: "SessionQuestionAnswers",
          id: `${arg.sessionId}-${arg.questionId}`,
        },
      ],
    }),
  }),
});

export const { useSubmitAnswerMutation, useGetSessionQuestionAnswersQuery } =
  answerApi;
