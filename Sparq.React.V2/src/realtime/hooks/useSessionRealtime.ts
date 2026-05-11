import { useEffect } from "react";
import { joinSessionGroup } from "../services/sessionsRealtimeService";

type Props = {
  sessionId: string;
  onParticipantsUpdated: () => void | Promise<void>;
  onSessionDeactivated?: () => void | Promise<void>;
};

export function useSessionRealtime({
  sessionId,
  onParticipantsUpdated,
  onSessionDeactivated,
}: Props) {
  useEffect(() => {
    let connectionRef: any;

    let participantsHandler: (() => void | Promise<void>) | null = null;

    let deactivatedHandler: (() => void | Promise<void>) | null = null;

    async function setup() {
      connectionRef = await joinSessionGroup(sessionId);

      participantsHandler = async () => {
        await onParticipantsUpdated();
      };

      deactivatedHandler = async () => {
        await onSessionDeactivated?.();
      };

      connectionRef.on("SessionParticipantsUpdated", participantsHandler);

      connectionRef.on("SessionDeactivated", deactivatedHandler);

      connectionRef.onreconnected(async () => {
        await connectionRef.invoke("JoinSessionGroup", sessionId);
      });
    }

    setup();

    return () => {
      if (!connectionRef) return;

      if (participantsHandler) {
        connectionRef.off("SessionParticipantsUpdated", participantsHandler);
      }

      if (deactivatedHandler) {
        connectionRef.off("SessionDeactivated", deactivatedHandler);
      }

      connectionRef.invoke("LeaveSessionGroup", sessionId).catch(() => {});
    };
  }, [sessionId, onParticipantsUpdated, onSessionDeactivated]);
}
