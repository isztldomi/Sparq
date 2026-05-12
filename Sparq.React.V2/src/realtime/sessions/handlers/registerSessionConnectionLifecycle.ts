import type { HubConnection } from "@microsoft/signalr";

import { joinSessionGroup } from "../sessionGroups";

type Params = {
  connection: HubConnection;

  sessionId: string;

  setIsConnected: React.Dispatch<React.SetStateAction<boolean>>;
};

export function registerSessionConnectionLifecycle({
  connection,
  sessionId,
  setIsConnected,
}: Params) {
  connection.onreconnected(async () => {
    await joinSessionGroup(sessionId);

    setIsConnected(true);
  });

  connection.onclose(() => {
    setIsConnected(false);
  });

  connection.onreconnecting(() => {
    setIsConnected(false);
  });
}
