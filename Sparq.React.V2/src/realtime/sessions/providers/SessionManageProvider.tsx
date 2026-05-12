import { useMemo } from "react";
import { Navigate } from "react-router-dom";

import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import { useGetSessionByIdQuery } from "@/features/session/sessionApi";

import { SessionManageContext } from "../context/SessionManageContext";

type Props = {
  sessionId?: string;
  children: React.ReactNode;
};

export function SessionManageProvider({ sessionId, children }: Props) {
  // 1. ALWAYS CALL HOOK FIRST (guard nélkül is)
  const queryResult = useGetSessionByIdQuery(sessionId ?? "");

  const { data, isLoading, isError } = queryResult;

  // 2. derived values
  const isReady = !!sessionId && !!data;

  const value = useMemo(() => {
    if (!sessionId || !data) return null;

    return {
      sessionId,
      sessionData: data,
    };
  }, [sessionId, data]);

  // 3. SAFE conditional rendering (hooks már lefutottak!)
  if (!sessionId) {
    return <Navigate to="/sessions/notFound" replace />;
  }

  if (isLoading) {
    return <LoadingIndicator />;
  }

  if (isError || !data || !value) {
    return <Navigate to="/sessions/notFound" replace />;
  }

  return (
    <SessionManageContext.Provider value={value}>
      {children}
    </SessionManageContext.Provider>
  );
}
