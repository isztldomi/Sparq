import type { RouteObject } from "react-router-dom";
import { RequireAuth } from "@/components/guards/RequireAuth";

import { HistoryPage } from "@/pages/HistoryPage";
import { MyQuizzesPage } from "@/pages/MyQuizzesPage";
import { QuizCreatePage } from "@/pages/QuizCreatePage";
import { QuizModifyPage } from "@/pages/QuizModifyPage";
import { QuizSessionsPage } from "@/pages/QuizSessionsPage";
import { RequireSessionAccess } from "@/components/guards/RequireSessionAccess";
import { SessionParticipantPage } from "@/pages/SessionParticipantPage";
import { SessionManagePage } from "@/pages/SessionManagePage";
import { SessionHistoryPage } from "@/pages/SessionHistoryPage";

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
    path: "/history/:sessionId",
    element: (
      <RequireAuth>
        <SessionHistoryPage />
      </RequireAuth>
    ),
  },
  {
    path: "/my-quizzes/:quizId/modify",
    element: (
      <RequireAuth>
        <QuizModifyPage />
      </RequireAuth>
    ),
  },
  {
    path: "/my-quizzes/:quizId/sessions",
    element: (
      <RequireAuth>
        <QuizSessionsPage />
      </RequireAuth>
    ),
  },
  {
    path: "/session/:sessionId",
    element: (
      <RequireSessionAccess>
        <SessionParticipantPage />
      </RequireSessionAccess>
    ),
  },
  {
    path: "/session/:sessionId/manage",
    element: (
      <RequireAuth>
        <SessionManagePage />
      </RequireAuth>
    ),
  },
];
