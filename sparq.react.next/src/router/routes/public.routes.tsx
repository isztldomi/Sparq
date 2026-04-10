import type { RouteObject } from "react-router-dom";

import { HomePage } from "@/pages/HomePage";
import { NotFoundPage } from "@/pages/NotFoundPage";
import { QuizzesPage } from "@/pages/QuizzesPage";
import { SnapshotPage } from "@/pages/SnapshotPage";
import { MyQuizzesPage } from "@/pages/MyQuizzesPage";

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
    path: "*",
    element: <NotFoundPage />,
  },
];
