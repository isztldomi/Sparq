import { createConnection } from "../createConnection";
import { getConnection, setConnection } from "../connectionStore";

const HUB_KEY = "sessions";

export async function getSessionsConnection() {
  let connection = getConnection(HUB_KEY);

  if (connection) {
    return connection;
  }

  connection = createConnection("/sessionsHub");

  await connection.start();

  setConnection(HUB_KEY, connection);

  return connection;
}

export async function joinSessionGroup(sessionId: string) {
  const connection = await getSessionsConnection();

  await connection.invoke("JoinSessionGroup", sessionId);

  return connection;
}
