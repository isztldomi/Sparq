import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "@/index.css";

import { RootLayout } from "@/pages/RootLayout";
import { HomePage } from "@/pages/HomePage";
import { NotFoundPage } from "@/pages/NotFoundPage";

const router = createBrowserRouter([
  {
    element: <RootLayout />,
    // errorElement: <ErrorPage />, // runtime error, jó esetben ilyen nem lesz
    children: [
      {
        path: "/",
        element: <HomePage />,
      },
      {
        path: "*", // csak 404
        element: <NotFoundPage />,
      },
    ],
  },
]);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
);
