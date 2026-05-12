import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import {
  activateForWaitingSessionApi,
  createSessionApi,
  deactivateSessionApi,
  deleteSessionApi,
  getAllPublicWaitingSessionsApi,
  getSessionByIdApi,
  getSessionPublicDataByIdApi,
  getSessionStatusByIdApi,
  joinSessionApi,
  quitSessionApi,
  startSessionApi,
} from "@/api/services/sessionService";

import type {
  CreatedSessionResponseDto,
  CreateSessionRequestDto,
  JoinSessionRequestDto,
  JoinSessionResponseDto,
  quitSessionRequestDto,
  SessionPublicWaitingListDto,
  SessionStatusResponseDto,
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
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: payload.quizId },
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
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: sessionId },
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
              { type: "Session", id: "PUBLIC_WAITING_LIST" },
              ...result.items.map((s) => ({
                type: "Session" as const,
                id: s.id,
              })),
            ]
          : [{ type: "Session", id: "PUBLIC_WAITING_LIST" }],
    }),

    getSessionById: builder.query<CreatedSessionResponseDto, string>({
      async queryFn(sessionId) {
        try {
          return {
            data: await getSessionByIdApi(sessionId),
          };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      providesTags: (result) =>
        result ? [{ type: "Session", id: result.id }] : [],
    }),

    getSessionPublicDataById: builder.query<
      SessionPublicWaitingListDto,
      string
    >({
      async queryFn(sessionId) {
        try {
          return {
            data: await getSessionPublicDataByIdApi(sessionId),
          };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      providesTags: (result) =>
        result ? [{ type: "Session", id: result.id }] : [],
    }),

    joinSession: builder.mutation<
      JoinSessionResponseDto,
      JoinSessionRequestDto
    >({
      async queryFn(data) {
        try {
          const response = await joinSessionApi(data);
          return { data: response };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },

      invalidatesTags: (_result, _error, arg) => [
        { type: "Session", id: arg.sessionId },
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: arg.sessionId },
      ],
    }),

    getSessionStatusById: builder.query<
      SessionStatusResponseDto,
      { sessionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          const response = await getSessionStatusByIdApi(sessionId, extUserId);
          return { data: response };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      providesTags: (_result, _error, { sessionId }) => [
        { type: "Session", id: sessionId },
        { type: "Participant", id: sessionId },
      ],
    }),

    quitSession: builder.mutation<boolean, quitSessionRequestDto>({
      async queryFn(data) {
        try {
          const response = await quitSessionApi(data);
          return { data: response };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      invalidatesTags: (_result, _error, arg) => [
        { type: "Session", id: arg.sessionId },
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: arg.sessionId },
      ],
    }),

    deleteSession: builder.mutation<boolean, string>({
      async queryFn(sessionId) {
        try {
          await deleteSessionApi(sessionId);
          return { data: true };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      invalidatesTags: (_result, _error, sessionId) => [
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: sessionId },
      ],
    }),
    deactivateSession: builder.mutation<boolean, string>({
      async queryFn(sessionId) {
        try {
          await deactivateSessionApi(sessionId);
          return { data: true };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      invalidatesTags: (_result, _error, sessionId) => [
        { type: "Session", id: sessionId },
        { type: "Session", id: "LIST" },
        { type: "Session", id: "PUBLIC_WAITING_LIST" },
        { type: "Participant", id: sessionId },
      ],
    }),
    startSession: builder.mutation<boolean, string>({
      async queryFn(sessionId) {
        try {
          await startSessionApi(sessionId);
          return { data: true };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
    }),
  }),
});

export const {
  useCreateSessionMutation,
  useActivateForWaitingSessionMutation,
  useGetAllPublicWaitingSessionsQuery,
  useGetSessionByIdQuery,
  useGetSessionPublicDataByIdQuery,
  useJoinSessionMutation,
  useGetSessionStatusByIdQuery,
  useQuitSessionMutation,
  useDeleteSessionMutation,
  useDeactivateSessionMutation,
  useStartSessionMutation,
} = sessionApi;
