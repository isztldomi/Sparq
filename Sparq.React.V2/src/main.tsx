import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { Provider } from "react-redux";
import { RouterProvider } from "react-router-dom";

import "./index.css";

import { router } from "@/router/router";
import { store } from "@/app/store";

import { AppBootstrap } from "./app/bootstrap/AppBootstrap";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
      <AppBootstrap />
      <RouterProvider router={router} />
    </Provider>
  </StrictMode>,
);
