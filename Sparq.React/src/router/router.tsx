import { createBrowserRouter } from "react-router-dom";
import { RootLayout } from "@/layouts/RootLayout";
import { publicRoutes } from "./routes/public.routes";

export const router = createBrowserRouter([
  {
    element: <RootLayout />,
    children: publicRoutes,
  },
]);
