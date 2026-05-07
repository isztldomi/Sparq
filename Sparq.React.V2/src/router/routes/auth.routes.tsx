import type { RouteObject } from "react-router-dom";
import { RequireAuth } from "@/components/guards/RequireAuth";

import { HistoryPage } from "@/pages/HistoryPage";
import { MyQuizzesPage } from "@/pages/MyQuizzesPage";
import { QuizCreatePage } from "@/pages/QuizCreatePage";
import { QuizModifyPage } from "@/pages/QuizModifyPage";

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
    path: "/my-quizzes/create",
    element: (
      <RequireAuth>
        <QuizCreatePage />
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
  {
    path: "/my-quizzes/:id/modify",
    element: (
      <RequireAuth>
        <QuizModifyPage />
      </RequireAuth>
    ),
  },
];
