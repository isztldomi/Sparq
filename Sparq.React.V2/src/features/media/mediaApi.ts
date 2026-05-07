import { baseApi } from "@/features/base/baseApi";
import { getMediaBlobApi, uploadMediaApi } from "@/api/services/mediaService";
import { toApiError } from "@/api/core/toApiError";

export const mediaApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    uploadMedia: builder.mutation<
      { id: number; fileName: string; contentType: string },
      File
    >({
      async queryFn(file) {
        try {
          const formData = new FormData();
          formData.append("file", file);

          return { data: await uploadMediaApi(formData) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      invalidatesTags: ["Media"],
    }),

    getMediaBlob: builder.query<Blob, string | number>({
      async queryFn(id) {
        try {
          return { data: await getMediaBlobApi(id) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: [],
    }),
  }),
});

export const {
  useUploadMediaMutation,
  useGetMediaBlobQuery,
  useLazyGetMediaBlobQuery,
} = mediaApi;
