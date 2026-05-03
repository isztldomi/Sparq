import type { RouteObject } from "react-router-dom";
import { RequireAuth } from "@/components/guards/RequireAuth";

import { HistoryPage } from "@/pages/HistoryPage";
import { MyQuizzesPage } from "@/pages/MyQuizzesPage";

export const authRoutes: RouteObject[] = [
  {
    path: "/my-quizzes",
    element: (
      <RequireAuth>
        <MyQuizzesPage />
      </RequireAuth>
    ),
  },
  {
    path: "/history",
    element: (
      <RequireAuth>
        <HistoryPage />
      </RequireAuth>
    ),
  },
];
