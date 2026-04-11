import type { RouteObject } from "react-router-dom";

import { HomePage } from "@/pages/HomePage";
import { NotFoundPage } from "@/pages/NotFoundPage";
import { QuizzesPage } from "@/pages/QuizzesPage";
import { SnapshotPage } from "@/pages/SnapshotPage";
import { MyQuizzesPage } from "@/pages/MyQuizzesPage";
import { QuizCreatePage } from "@/pages/QuizCreatePage";
import { TempAnswerPage } from "@/pages/temp/TempAnswerPage";

export const publicRoutes: RouteObject[] = [
  {
    index: true,
    element: <HomePage />,
  },
  {
    path: "/quizzes",
    element: <QuizzesPage />,
  },
  {
    path: "/snapshot/:id",
    element: <SnapshotPage />,
  },
  {
    path: "/my-quizzes",
    element: <MyQuizzesPage />,
  },
  {
    path: "/quiz/create",
    element: <QuizCreatePage />,
  },
  {
    path: "/demo/snapshot/:snapshotId",
    element: <TempAnswerPage />,
  },
  {
    path: "*",
    element: <NotFoundPage />,
  },
];
