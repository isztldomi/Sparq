import { store } from "@/app/store";
import { setAuth } from "@/features/auth/authSlice";

export function initializeAuth() {
  const auth = localStorage.getItem("auth");

  if (!auth) {
    return;
  }

  try {
    const parsed = JSON.parse(auth);

    store.dispatch(
      setAuth({
        token: parsed.token,
        refreshToken: parsed.refreshToken,
      }),
    );
  } catch {
    localStorage.removeItem("auth");
  }
}
