import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import {
  extUserIsJoinedApi,
  isJoinedApi,
} from "@/api/services/participantService";
import type { ParticipantIsJoinedResponseDto } from "./participantTypes";

export const participantApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    isJoined: builder.query<ParticipantIsJoinedResponseDto, string>({
      async queryFn(sessionId) {
        try {
          return { data: await isJoinedApi(sessionId) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: (_result, _error, sessionId) => [
        { type: "Session", id: sessionId },
      ],
    }),
    extUserIsJoined: builder.query<
      ParticipantIsJoinedResponseDto,
      { sessionId: string; extUserId: string }
    >({
      async queryFn({ sessionId, extUserId }) {
        try {
          return { data: await extUserIsJoinedApi(sessionId, extUserId) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: (_result, _error, { sessionId }) => [
        { type: "Session", id: sessionId },
      ],
    }),
  }),
});

export const { useIsJoinedQuery, useExtUserIsJoinedQuery } = participantApi;
