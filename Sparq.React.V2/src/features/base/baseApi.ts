import { createApi } from "@reduxjs/toolkit/query/react";

export const baseApi = createApi({
  reducerPath: "api",
  baseQuery: async () => ({ data: {} }),
  tagTypes: ["User", "Quiz", "Media", "Session", "Participant", "Question"],
  endpoints: () => ({}),
});
