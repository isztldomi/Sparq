import { type ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useGetProfileQuery } from "@/features/user/userApi";
import { LoadingIndicator } from "@/components/LoadingIndicator";

type RequireAuthProps = {
  children: ReactNode;
  redirectTo?: string;
};

export const RequireAuth = ({
  children,
  redirectTo = "/login",
}: RequireAuthProps) => {
  const location = useLocation();

  const { data: user, isLoading, isError } = useGetProfileQuery();

  // 1. még tölt → várunk
  if (isLoading) {
    return <LoadingIndicator />;
  }

  // 2. nincs user → nem auth
  if (!user || isError) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  // 3. minden oké
  return <>{children}</>;
};
