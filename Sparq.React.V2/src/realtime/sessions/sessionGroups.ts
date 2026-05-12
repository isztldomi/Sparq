import { getSessionsHub } from "./sessionsHub";

export async function joinSessionGroup(sessionId: string) {
  const connection = await getSessionsHub();

  await connection.invoke("JoinSessionGroup", sessionId);

  return connection;
}

export async function leaveSessionGroup(sessionId: string) {
  const connection = await getSessionsHub();

  await connection.invoke("LeaveSessionGroup", sessionId);
}
