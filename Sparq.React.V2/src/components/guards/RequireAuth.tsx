import { type ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";

import { useAppSelector } from "@/app/hooks";
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

  const auth = useAppSelector((state) => state.auth);
  const user = useAppSelector((state) => state.user);

  // DEBUG LOGS
  //console.log("AUTH STATE:", auth);
  //console.log("USER STATE:", user);

  if (auth.loading) {
    //console.log("AUTH: loading...");
    return <LoadingIndicator />;
  }

  if (!user.user) {
    //console.log("AUTH: no user → redirecting to login");
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  //console.log("AUTH: allowed -> rendering children");

  return <>{children}</>;
};
