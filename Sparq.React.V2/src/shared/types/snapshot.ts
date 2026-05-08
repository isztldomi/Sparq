import type { Question } from "./question";

export type Snapshot = {
  id: string;
  quizId: string;
  snapshotNumber: number;
  title: string;
  description: string;
  timeLimit: number;
  pinCode: string;
  createdAt: Date;
  questions: Question[];
  //sessions: Session[];
};
