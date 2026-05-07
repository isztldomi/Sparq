import { baseApi } from "@/features/base/baseApi";
import { createSnapshotApi } from "@/api/services/snapshotService";
import { toApiError } from "@/api/core/toApiError";
import type {
  SnapshotCreateRequestDto,
  SnapshotResponseDto,
} from "./snapshotTypes";

export const snapshotApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    createSnapshot: builder.mutation<
      SnapshotResponseDto,
      SnapshotCreateRequestDto
    >({
      async queryFn(payload) {
        try {
          return { data: await createSnapshotApi(payload) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      invalidatesTags: (_r, _e, payload) => [
        { type: "Quiz", id: payload.quizId },
        { type: "Quiz", id: "LIST" },
      ],
    }),
  }),
});

export const { useCreateSnapshotMutation } = snapshotApi;
