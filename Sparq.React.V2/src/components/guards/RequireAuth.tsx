import { type ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useGetProfileQuery } from "@/features/user/userApi";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

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

  if (isLoading) {
    return <LoadingIndicator />;
  }

  if (!user || isError) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  return <>{children}</>;
};
