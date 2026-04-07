import { type ReactNode, useState, useEffect, useCallback } from "react";
import type { LoginRequestDto } from "@/api/models/loginDto/LoginRequestDto";
import type { LoginResponseDto } from "@/api/models/loginDto/LoginResponseDto";
import { jwtDecode } from "jwt-decode";
import {
  UserContext,
  type UserContextModel,
  type UserInfo,
} from "@/contexts/UserContext";

// eslint-disable @typescript-eslint/no-unused-vars */
// eslint-disable react-hooks/exhaustive-deps */

export function UserContextProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserInfo | null>(null);
  const [authError, setAuthError] = useState<string | null>(null);
  const [initialized, setInitialized] = useState<boolean>(false);

  const loggedIn = user !== null;

  // helper: persist user
  const persistUser = (user: UserInfo) => {
    localStorage.setItem("authToken", user.authToken);
    localStorage.setItem("refreshToken", user.refreshToken);
    localStorage.setItem("user", JSON.stringify(user));
  };

  const clearStorage = () => {
    localStorage.removeItem("authToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
  };

  const handleLoginResponse = useCallback((response: LoginResponseDto) => {
    const decoded = jwtDecode<{ exp: number }>(response.authToken);

    const userInfo: UserInfo = {
      userId: response.userId,
      authToken: response.authToken,
      refreshToken: response.refreshToken,
      authTokenExpiration: decoded.exp * 1000,
    };

    setUser(userInfo);
    setAuthError(null);

    persistUser(userInfo);
  }, []);

  const redeemToken = useCallback(
    async (refreshToken: string) => {
      try {
        const res = await fetch("/api/users/refresh", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ refreshToken }),
        });

        if (!res.ok) {
          throw new Error("Refresh failed");
        }

        const response = (await res.json()) as LoginResponseDto;
        handleLoginResponse(response);
      } catch {
        setUser(null);
        setAuthError("Session expired");
        clearStorage();
      }
    },
    [handleLoginResponse],
  );

  const handleLogin = useCallback(
    async (data: LoginRequestDto) => {
      try {
        setAuthError(null);

        const res = await fetch("/api/users/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });

        if (!res.ok) {
          throw new Error("Login failed");
        }

        const response = (await res.json()) as LoginResponseDto;
        handleLoginResponse(response);
      } catch {
        setAuthError("Invalid email or password");
        setUser(null);
      }
    },
    [handleLoginResponse],
  );

  const handleLogout = useCallback(async () => {
    try {
      const refreshToken = localStorage.getItem("refreshToken");

      if (refreshToken) {
        await fetch("/api/users/logout", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ refreshToken }),
        });
      }
    } catch {
      // ignore logout errors
    } finally {
      setUser(null);
      setAuthError(null);
      clearStorage();
    }
  }, []);

  useEffect(() => {
    async function initialize() {
      try {
        const storedUser = localStorage.getItem("user");
        const refreshToken = localStorage.getItem("refreshToken");

        if (refreshToken) {
          await redeemToken(refreshToken);
        } else if (storedUser) {
          const parsed = JSON.parse(storedUser) as UserInfo;
          setUser(parsed);
        }
      } finally {
        setInitialized(true);
      }
    }

    initialize();
  }, [redeemToken]);

  useEffect(() => {
    if (!user) return;

    const timeout = user.authTokenExpiration - Date.now() - 60_000;

    if (timeout <= 0) {
      redeemToken(user.refreshToken);
      return;
    }

    const timer = setTimeout(() => {
      redeemToken(user.refreshToken);
    }, timeout);

    return () => clearTimeout(timer);
  }, [user, redeemToken]);

  const contextValue: UserContextModel = {
    userId: user ? user.userId : null,
    loggedIn,
    initialized,
    authError,
    handleLogin,
    handleLogout,
  };

  return (
    <UserContext.Provider value={contextValue}>{children}</UserContext.Provider>
  );
}
