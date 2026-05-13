import { baseApi } from "@/features/base/baseApi";
import {
  getMediaBlobApi,
  getMediaBlobSessionApi,
  uploadMediaApi,
} from "@/api/services/mediaService";
import { toApiError } from "@/api/core/toApiError";

export const mediaApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    uploadMedia: builder.mutation<
      { id: string; fileName: string; contentType: string },
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

    getMediaBlob: builder.query<Blob, string | string>({
      async queryFn(id) {
        try {
          return { data: await getMediaBlobApi(id) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: [],
    }),
    getMediaBlobSession: builder.query<
      Blob,
      { sessionId: string; mediaId: string; extUserId?: string }
    >({
      async queryFn({ sessionId, mediaId, extUserId }) {
        try {
          return {
            data: await getMediaBlobSessionApi(sessionId, mediaId, extUserId),
          };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
    }),
  }),
});

export const {
  useUploadMediaMutation,
  useGetMediaBlobQuery,
  useLazyGetMediaBlobQuery,
  useGetMediaBlobSessionQuery,
} = mediaApi;
