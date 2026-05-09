import type { HubConnection } from "@microsoft/signalr";

const connections = new Map<string, HubConnection>();

export function getConnection(key: string) {
  return connections.get(key);
}

export function setConnection(key: string, connection: HubConnection) {
  connections.set(key, connection);
}
