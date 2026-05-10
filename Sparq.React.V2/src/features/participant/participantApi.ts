import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import {
  getParticipantsBySessionIdApi,
  isJoinedApi,
} from "@/api/services/participantService";
import type {
  ParticipantIsJoinedResponseDto,
  ParticipantPublicListResponseDto,
} from "./participantTypes";

export const participantApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    isJoined: builder.query<
      ParticipantIsJoinedResponseDto,
      { sessionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          return {
            data: await isJoinedApi(sessionId, extUserId),
          };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: (_result, _error, { sessionId }) => [
        { type: "Session", id: sessionId },
      ],
    }),
    getParticipantsBySessionId: builder.query<
      ParticipantPublicListResponseDto[],
      { sessionId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          return {
            data: await getParticipantsBySessionIdApi(sessionId, extUserId),
          };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: (result, _error, { sessionId }) =>
        result
          ? [
              { type: "Participant", id: sessionId },
              ...result.map((p) => ({
                type: "Participant" as const,
                id: p.id,
              })),
            ]
          : [{ type: "Participant", id: sessionId }],
    }),
  }),
});

export const { useIsJoinedQuery, useGetParticipantsBySessionIdQuery } =
  participantApi;
