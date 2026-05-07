import type { AuthData } from "./authTypes";

const AUTH_KEY = "auth";

export function getAuth(): AuthData | null {
  const raw = localStorage.getItem(AUTH_KEY);

  if (!raw) return null;

  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

export function setAuth(data: AuthData) {
  localStorage.setItem(AUTH_KEY, JSON.stringify(data));
}

export function clearAuth() {
  localStorage.removeItem(AUTH_KEY);
}
