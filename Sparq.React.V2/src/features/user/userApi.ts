import { baseApi } from "@/features/base/baseApi";
import {
  getCurrentUserApi,
  getProfileApi,
  updateNickNameApi,
} from "@/api/services/userService";
import { mapUser } from "@/features/user/userMapper";
import type { User } from "@/shared/types/user";
import type { NickNameUpdateRequestDto } from "@/features/user/userTypes";
import { toApiError } from "@/api/core/toApiError";

export const userApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getProfile: builder.query<User, void>({
      async queryFn() {
        try {
          return { data: mapUser(await getProfileApi()) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      providesTags: ["User"],
    }),
    getCurrentUser: builder.query<User | null, void>({
      async queryFn() {
        try {
          const result = await getCurrentUserApi();

          return {
            data: result ? mapUser(result) : null,
          };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },

      providesTags: ["User"],
    }),

    updateNickName: builder.mutation<User, NickNameUpdateRequestDto>({
      async queryFn(data) {
        try {
          return { data: mapUser(await updateNickNameApi(data)) };
        } catch (e) {
          return { error: toApiError(e) };
        }
      },
      invalidatesTags: ["User"],
    }),
  }),
});

export const {
  useGetProfileQuery,
  useGetCurrentUserQuery,
  useUpdateNickNameMutation,
} = userApi;
