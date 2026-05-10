import { useEffect } from "react";
import { joinSessionGroup } from "../services/sessionsRealtimeService";

type Props = {
  sessionId: string;
  onParticipantsUpdated: () => void | Promise<void>;
};

export function useSessionRealtime({
  sessionId,
  onParticipantsUpdated,
}: Props) {
  useEffect(() => {
    let connectionRef: any;
    let handler: (() => void) | null = null;

    async function setup() {
      connectionRef = await joinSessionGroup(sessionId);

      handler = async () => {
        await onParticipantsUpdated();
      };

      connectionRef.on("SessionParticipantsUpdated", handler);

      connectionRef.onreconnected(async () => {
        await connectionRef.invoke("JoinSessionGroup", sessionId);
      });
    }

    setup();

    return () => {
      if (!connectionRef || !handler) return;

      connectionRef.off("SessionParticipantsUpdated", handler);

      connectionRef.invoke("LeaveSessionGroup", sessionId).catch(() => {});
    };
  }, [sessionId, onParticipantsUpdated]);
}
