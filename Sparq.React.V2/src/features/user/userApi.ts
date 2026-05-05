import { createApi } from "@reduxjs/toolkit/query/react";
import { getProfileApi, updateNickNameApi } from "@/api/services/userService";
import { mapUser } from "@/features/user/userMapper";
import type { User } from "@/shared/types/user";
import type { NickNameUpdateRequestDto } from "@/features/user/userTypes";
import { toApiError } from "@/api/core/toApiError";

export const userApi = createApi({
  reducerPath: "userApi",

  baseQuery: async () => ({ data: {} }),

  tagTypes: ["User"],

  endpoints: (builder) => ({
    getProfile: builder.query<User, void>({
      async queryFn() {
        try {
          const dto = await getProfileApi();
          return { data: mapUser(dto) };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      providesTags: ["User"],
    }),

    updateNickName: builder.mutation<User, NickNameUpdateRequestDto>({
      async queryFn(data) {
        try {
          const dto = await updateNickNameApi(data);
          return { data: mapUser(dto) };
        } catch (e) {
          return {
            error: toApiError(e),
          };
        }
      },
      invalidatesTags: ["User"],
    }),
  }),
});

export const { useGetProfileQuery, useUpdateNickNameMutation } = userApi;
