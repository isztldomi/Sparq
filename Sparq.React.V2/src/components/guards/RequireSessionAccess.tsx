import { type ReactNode } from "react";
import { Navigate, useLocation, useParams } from "react-router-dom";
import { useGetCurrentUserQuery } from "@/features/user/userApi";
import { useIsJoinedQuery } from "@/features/participant/participantApi";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

type Props = {
  children: ReactNode;
};

export function RequireSessionAccess({ children }: Props) {
  const location = useLocation();
  const { sessionId } = useParams();

  const { data: user, isLoading: userLoading } = useGetCurrentUserQuery();

  const isLoggedIn = !!user;

  const extUserId = sessionId ? localStorage.getItem(sessionId) : undefined;

  const { data: joinedData, isLoading: joinedLoading } = useIsJoinedQuery(
    {
      sessionId: sessionId!,
      extUserId: isLoggedIn ? undefined : (extUserId ?? undefined),
    },
    {
      skip: !sessionId,
    },
  );

  const loading = userLoading || joinedLoading;

  if (loading) {
    return <LoadingIndicator />;
  }

  const isJoined = joinedData?.isJoined;

  if (!isJoined) {
    return (
      <Navigate
        to={`/session/${sessionId}/join`}
        state={{ from: location }}
        replace
      />
    );
  }

  return <>{children}</>;
}
