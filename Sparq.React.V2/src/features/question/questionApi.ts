import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";

import {
  getCurrentQuestionWithoutResultApi,
  getCurrentQuestionWithResultApi,
} from "@/api/services/questionService";

import type {
  CurrentSessionQuestionStateWithoutResultDto,
  CurrentSessionQuestionStateWithResultDto,
} from "./questionTypes";

export const questionApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getCurrentQuestionWithoutResult: builder.query<
      CurrentSessionQuestionStateWithoutResultDto,
      { sessionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          const data = await getCurrentQuestionWithoutResultApi(
            sessionId,
            extUserId,
          );

          return { data };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (_result, _error, { sessionId }) => [
        { type: "Question", id: `${sessionId}-current-without-result` },
      ],
    }),

    getCurrentQuestionWithResult: builder.query<
      CurrentSessionQuestionStateWithResultDto,
      { sessionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          const data = await getCurrentQuestionWithResultApi(
            sessionId,
            extUserId,
          );

          return { data };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: (_result, _error, { sessionId }) => [
        { type: "Question", id: `${sessionId}-current-with-result` },
      ],
    }),
  }),
});

export const {
  useGetCurrentQuestionWithoutResultQuery,
  useGetCurrentQuestionWithResultQuery,
} = questionApi;
