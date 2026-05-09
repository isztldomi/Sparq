import type { RouteObject } from "react-router-dom";

import { HomePage } from "@/pages/HomePage";
import { NotFoundPage } from "@/pages/NotFoundPage";
import { ProfilePage } from "@/pages/ProfilePage";
import { LoginPage } from "@/pages/LoginPage";
import { LogoutPage } from "@/pages/LogoutPage";
import { RegistrationPage } from "@/pages/RegistrationPage";
import { SessionPage } from "@/pages/SessionPage";
import { PublicActiveSessionsPage } from "@/pages/PublicActiveSessionsPage";

export const publicRoutes: RouteObject[] = [
  {
    index: true,
    element: <HomePage />,
  },
  {
    path: "/profile",
    element: <ProfilePage />,
  },
  {
    path: "/profile/login",
    element: <LoginPage />,
  },
  {
    path: "/profile/logout",
    element: <LogoutPage />,
  },
  {
    path: "/profile/register",
    element: <RegistrationPage />,
  },
  {
    path: "/session",
    element: <PublicActiveSessionsPage />,
  },
  {
    path: "/session/:sessionId",
    element: <SessionPage />,
  },
  {
    path: "*",
    element: <NotFoundPage />,
  },
];
