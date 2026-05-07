import { useEffect } from "react";

import { setupInterceptors } from "@/api/client/interceptors";
import { initializeAuth } from "@/app/bootstrap/initializeAuth";

let initialized = false;

export function AppBootstrap() {
  useEffect(() => {
    if (initialized) {
      return;
    }

    initialized = true;

    setupInterceptors();
    initializeAuth();
  }, []);

  return null;
}

// STRICT MÓD MIATT NEM JÓ
// import { setupInterceptors } from "@/api/client/interceptors";
// import { initializeAuth } from "@/app/bootstrap/initializeAuth";
//
// export function AppBootstrap() {
//   setupInterceptors();
//   initializeAuth();
//
//   return null;
// }
