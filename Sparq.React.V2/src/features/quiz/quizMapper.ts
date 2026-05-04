import type { QuizCreateRequestDto } from "@/features/quiz/quizTypes";
import type { QuizUI } from "@/features/quiz/quizTypes";
import { mapSnapshotUIToDto } from "@/features/snapshot/snapshotMapper";

export function mapQuizUIToDto(ui: QuizUI): QuizCreateRequestDto {
  return {
    isPublic: ui.isPublic,
    snapshots: ui.snapshots.map(mapSnapshotUIToDto),
  };
}
