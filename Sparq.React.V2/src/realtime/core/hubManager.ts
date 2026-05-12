import type { HubConnection } from "@microsoft/signalr";

const connections = new Map<string, HubConnection>();

export function getHubConnection(key: string) {
  return connections.get(key);
}

export function setHubConnection(key: string, connection: HubConnection) {
  connections.set(key, connection);
}
