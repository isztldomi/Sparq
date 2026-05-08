import type { Answer } from "./answer";

export type Question = {
  id: string;
  snapshotId: string;
  title: string;
  text: string;
  mediaUrl: string;
  timeLimit: number;
  point: number;
  answers: Answer[];
  // participantAnswers: ParticipantAnswer[];
  // messages: Message[];
};
