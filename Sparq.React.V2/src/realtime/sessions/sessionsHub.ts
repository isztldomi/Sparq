import { createHubConnection } from "../core/createHubConnection";
import { getHubConnection, setHubConnection } from "../core/hubManager";

const HUB_NAME = "sessions";
const HUB_PATH = "/sessionsHub";

export async function getSessionsHub() {
  let connection = getHubConnection(HUB_NAME);

  if (connection) {
    return connection;
  }

  connection = createHubConnection(HUB_PATH);

  await connection.start();

  setHubConnection(HUB_NAME, connection);

  return connection;
}
