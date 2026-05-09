import { createConnection } from "@/realtime/createConnection";
import { getConnection, setConnection } from "@/realtime/connectionStore";

const SESSION_CONNECTION_KEY = "session";

export function getSessionConnection() {
  const existing = getConnection(SESSION_CONNECTION_KEY);

  if (existing) {
    return existing;
  }

  const connection = createConnection("/sessionHub");

  setConnection(SESSION_CONNECTION_KEY, connection);

  return connection;
}
