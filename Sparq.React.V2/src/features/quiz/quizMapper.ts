import type {
  QuizCreateRequestDto,
  QuizResponseDto,
} from "@/features/quiz/quizTypes";
import type { QuizUI } from "@/features/quiz/quizTypes";
import {
  mapSnapshotUIToSnapshotCreateFromQuizRequestDto,
  mapSnapshotDtoToUI,
} from "@/features/snapshot/snapshotMapper";

export function mapQuizUIToDto(ui: QuizUI): QuizCreateRequestDto {
  return {
    isPublic: ui.isPublic,
    snapshots: ui.snapshots.map(
      mapSnapshotUIToSnapshotCreateFromQuizRequestDto,
    ),
  };
}

export function mapQuizDtoToUI(dto: QuizResponseDto): QuizUI {
  return {
    isPublic: dto.isPublic,
    snapshots: [mapSnapshotDtoToUI(dto.lastSnapshot)],
  };
}
