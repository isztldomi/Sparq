import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "@/app/hooks";
import { resetUser } from "@/features/user/user.slice";
import { logout } from "@/features/auth/auth.slice";

export function LogoutPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(logout());
    dispatch(resetUser());
    navigate("/profile");
  }, [dispatch, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <h1>Logging out...</h1>
    </div>
  );
}
