import type { RouteObject } from "react-router-dom";
import { LoginPage } from "@/pages/user/LoginPage";
import { LogoutPage } from "@/pages/user/LogoutPage";

export const authRoutes: RouteObject[] = [
  {
    path: "/user/login",
    element: <LoginPage />,
  },
  {
    path: "/user/logout",
    element: <LogoutPage />,
  },
];
