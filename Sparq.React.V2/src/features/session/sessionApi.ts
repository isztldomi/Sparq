import { baseApi } from "@/features/base/baseApi";
import { toApiError } from "@/api/core/toApiError";
import { createSessionApi } from "@/api/services/sessionService";

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
  }),
});

export const { useCreateSessionMutation } = sessionApi;
