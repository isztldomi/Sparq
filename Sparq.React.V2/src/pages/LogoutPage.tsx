import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "@/app/hooks";
import { logout } from "@/features/auth/authSlice";
import { authApi } from "@/features/auth/authApi";
import { userApi } from "@/features/user/userApi";

export function LogoutPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    const run = async () => {
      // 1. auth state
      dispatch(logout());

      // 2. cache cleanup
      dispatch(authApi.util.resetApiState());
      dispatch(userApi.util.resetApiState());

      // 3. storage cleanup (extra biztonság)
      localStorage.removeItem("auth");

      // 4. redirect
      navigate("/login", { replace: true });
    };

    run();
  }, [dispatch, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <h1>Logging out...</h1>
    </div>
  );
}
