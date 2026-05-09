import { type ReactNode } from "react";
import { Navigate, useLocation, useParams } from "react-router-dom";
import { useGetCurrentUserQuery } from "@/features/user/userApi";
import {
  useIsJoinedQuery,
  useExtUserIsJoinedQuery,
} from "@/features/participant/participantApi";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";

type Props = {
  children: ReactNode;
};

export function RequireSessionAccess({ children }: Props) {
  const location = useLocation();
  const { sessionId } = useParams();

  const { data: user, isLoading: userLoading } = useGetCurrentUserQuery();

  const isLoggedIn = !!user;
  const extUserId = sessionId ? localStorage.getItem(sessionId) : null;

  const { data: joinedData, isLoading: joinedLoading } = useIsJoinedQuery(
    sessionId!,
    {
      skip: !sessionId || !isLoggedIn,
    },
  );

  const { data: extJoinedData, isLoading: extLoading } =
    useExtUserIsJoinedQuery(
      { sessionId: sessionId!, extUserId: extUserId! },
      {
        skip: !sessionId || isLoggedIn || !extUserId,
      },
    );

  const loading = userLoading || joinedLoading || extLoading;

  if (loading) {
    return <LoadingIndicator />;
  }

  const isJoined = isLoggedIn ? joinedData?.isJoined : extJoinedData?.isJoined;

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
