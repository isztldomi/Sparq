import { useEffect } from "react";
import { useAppDispatch } from "@/app/hooks";
import { fetchProfile } from "@/features/auth/auth.thunks";

export function AppBootstrap() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const auth = localStorage.getItem("auth");

    if (auth) {
      dispatch(fetchProfile());
    }
  }, [dispatch]);

  return null;
}
