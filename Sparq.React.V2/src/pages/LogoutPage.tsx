import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useAppDispatch } from "@/app/hooks";

import { clearAuth } from "@/features/auth/authStorage";
import { baseApi } from "@/features/base/baseApi";

export function LogoutPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    clearAuth();

    dispatch(baseApi.util.resetApiState());

    navigate("/profile", {
      replace: true,
    });
  }, [dispatch, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <h1>Logging out...</h1>
    </div>
  );
}
