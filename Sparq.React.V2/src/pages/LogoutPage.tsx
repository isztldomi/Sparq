import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useAppDispatch } from "@/app/hooks";

import { authApi } from "@/features/auth/authApi";
import { userApi } from "@/features/user/userApi";
import { quizApi } from "@/features/quiz/quizApi";
import { mediaApi } from "@/features/media/mediaApi";

import { clearAuth } from "@/features/auth/authStorage";
import { snapshotApi } from "@/features/snapshot/snapshotApi";

export function LogoutPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    // auth cleanup
    clearAuth();

    // RTK Query cache cleanup
    dispatch(authApi.util.resetApiState());
    dispatch(userApi.util.resetApiState());
    dispatch(quizApi.util.resetApiState());
    dispatch(mediaApi.util.resetApiState());
    dispatch(snapshotApi.util.resetApiState());

    // redirect
    navigate("/profile/login", {
      replace: true,
    });
  }, [dispatch, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <h1>Logging out...</h1>
    </div>
  );
}
