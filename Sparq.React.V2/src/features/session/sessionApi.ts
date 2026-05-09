import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import {
  activateForWaitingSessionApi,
  createSessionApi,
} from "@/api/services/sessionService";

import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
} from "./sessionTypes";

export const sessionApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    createSession: builder.mutation<
      CreatedSessionResponseDto,
      CreateSessionRequestDto
    >({
      async queryFn(data) {
        try {
          return {
            data: await createSessionApi(data),
          };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },

      invalidatesTags: (_result, _error, payload) => [
        { type: "Session", id: payload.quizId },
        { type: "Session", id: "LIST" },
      ],
    }),
    activateForWaitingSession: builder.mutation<void, string>({
      async queryFn(sessionId) {
        try {
          await activateForWaitingSessionApi(sessionId);
          return { data: undefined };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      invalidatesTags: (_result, _error, sessionId) => [
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
      ],
    }),
  }),
});

export const {
  useCreateSessionMutation,
  useActivateForWaitingSessionMutation,
} = sessionApi;
