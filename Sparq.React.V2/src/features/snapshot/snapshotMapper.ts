import type { SnapshotCreateFromQuizRequestDto } from "@/features/snapshot/snapshotTypes";
import type { SnapshotUI } from "@/features/snapshot/snapshotTypes";
import { mapQuestionUIToDto } from "@/features/question/questionMapper";

export function mapSnapshotUIToDto(
  s: SnapshotUI,
): SnapshotCreateFromQuizRequestDto {
  return {
    title: s.title,
    description: s.description,
    timeLimit: s.timeLimit,
    pinCode: s.pinCode,
    questions: s.questions.map(mapQuestionUIToDto),
  };
}
