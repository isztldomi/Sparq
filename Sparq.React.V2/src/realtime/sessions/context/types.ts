import type { HubConnection } from "@microsoft/signalr";

// import type { useGetSessionByIdQuery } from "@/features/session/sessionApi";

export type SessionRealtimeContextValue = {
  sessionId: string;

  connection: HubConnection | null;

  isConnected: boolean;
};

// export type SessionManageContextValue = {
//   sessionId: string;
//
//   sessionData: ReturnType<typeof useGetSessionByIdQuery>["data"];
// };
export type SessionManageContextValue = {
  sessionId: string;
  sessionData: any; // RTK type
};
