import type { Answer } from "./answer";

export type Question = {
  id: number;
  snapshotId: number;
  title: string;
  text: string;
  mediaUrl: string;
  timeLimit: number;
  point: number;
  answers: Answer[];
  // participantAnswers: ParticipantAnswer[];
  // messages: Message[];
};
