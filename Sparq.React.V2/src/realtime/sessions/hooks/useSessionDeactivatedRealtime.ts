import { useHubEvent } from "../../core/hooks/useHubEvent";

import { SESSION_EVENTS } from "../sessionEvents";

export function useSessionDeactivatedRealtime(
  connection: any,
  callback?: () => void | Promise<void>,
) {
  useHubEvent({
    connection,
    event: SESSION_EVENTS.SESSION_DEACTIVATED,
    handler: async () => {
      await callback?.();
    },
  });
}
