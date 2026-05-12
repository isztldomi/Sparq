import type { HubConnection } from "@microsoft/signalr";

import { joinSessionGroup } from "../sessionGroups";

export async function createSessionConnection(
  sessionId: string,
): Promise<HubConnection> {
  return await joinSessionGroup(sessionId);
}
