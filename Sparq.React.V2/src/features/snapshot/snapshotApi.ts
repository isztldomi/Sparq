import { createApi } from "@reduxjs/toolkit/query/react";
import type {
  SnapshotCreateRequestDto,
  SnapshotResponseDto,
} from "./snapshotTypes";
import { createSnapshotApi } from "@/api/services/snapshotService";
import { toApiError } from "@/api/core/toApiError";

export const snapshotApi = createApi({
  reducerPath: "snapshotApi",
  baseQuery: async () => ({ data: {} }),

  tagTypes: ["Snapshot"],

  endpoints: (builder) => ({
    createSnapshot: builder.mutation<
      SnapshotResponseDto,
      SnapshotCreateRequestDto
    >({
      async queryFn(payload) {
        try {
          const dto = await createSnapshotApi(payload);
          return { data: dto };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      invalidatesTags: ["Snapshot"],
    }),
  }),
});

export const { useCreateSnapshotMutation } = snapshotApi;
