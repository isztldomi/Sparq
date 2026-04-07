import type { RouteObject } from "react-router-dom";

import { HomePage } from "@/pages/HomePage";
import { NotFoundPage } from "@/pages/NotFoundPage";
import { QuizzesPage } from "@/pages/QuizzesPage";

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
    path: "*",
    element: <NotFoundPage />,
  },
];
