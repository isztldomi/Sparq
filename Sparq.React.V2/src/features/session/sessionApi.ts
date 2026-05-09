import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import {
  activateForWaitingSessionApi,
  createSessionApi,
  getAllPublicWaitingSessionsApi,
} from "@/api/services/sessionService";

import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
  SessionPublicWaitingListDto,
} from "./sessionTypes";
import type { PagedResult } from "../page/pageTypes";

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
    getAllPublicWaitingSessions: builder.query<
      PagedResult<SessionPublicWaitingListDto>,
      { page: number; pageSize: number }
    >({
      async queryFn({ page, pageSize }) {
        try {
          return {
            data: await getAllPublicWaitingSessionsApi(page, pageSize),
          };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      providesTags: (result) =>
        result
          ? [
              ...result.items.map((s) => ({
                type: "Session" as const,
                id: s.id,
              })),
              { type: "Session" as const, id: "LIST" },
            ]
          : [{ type: "Session" as const, id: "LIST" }],
    }),
  }),
});

export const {
  useCreateSessionMutation,
  useActivateForWaitingSessionMutation,
  useGetAllPublicWaitingSessionsQuery,
} = sessionApi;
