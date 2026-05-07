import { getAuth, clearAuth } from "@/features/auth/authStorage";

export function initializeAuth() {
  const auth = getAuth();

  if (!auth) {
    return;
  }

  try {
    // ha valami sérült lenne, itt kiszűrjük
    if (!auth.token || !auth.refreshToken) {
      clearAuth();
    }
  } catch {
    clearAuth();
  }
}
