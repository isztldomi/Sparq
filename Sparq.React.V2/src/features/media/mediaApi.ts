import { createApi } from "@reduxjs/toolkit/query/react";
import { uploadMediaApi } from "@/api/services/mediaService";
import { toApiError } from "@/api/core/toApiError";

export const mediaApi = createApi({
  reducerPath: "mediaApi",
  baseQuery: async () => ({ data: {} }),
  tagTypes: ["Media"],

  endpoints: (builder) => ({
    uploadMedia: builder.mutation<
      { id: number; fileName: string; contentType: string },
      File
    >({
      async queryFn(file) {
        try {
          const formData = new FormData();
          formData.append("file", file);

          const dto = await uploadMediaApi(formData);

          return { data: dto };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
    }),
  }),
});

export const { useUploadMediaMutation } = mediaApi;
