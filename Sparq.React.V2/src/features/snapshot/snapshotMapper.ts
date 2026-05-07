import type {
  SnapshotCreateFromQuizRequestDto,
  SnapshotCreateRequestDto,
  SnapshotResponseDto,
} from "@/features/snapshot/snapshotTypes";
import type { SnapshotUI } from "@/features/snapshot/snapshotTypes";
import {
  mapQuestionUIToDto,
  mapQuestionDtoToUI,
} from "@/features/question/questionMapper";

export function mapSnapshotUIToSnapshotCreateFromQuizRequestDto(
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

export function mapSnapshotDtoToUI(s: SnapshotResponseDto): SnapshotUI {
  return {
    title: s.title,
    description: s.description,
    timeLimit: s.timeLimit,
    pinCode: s.pinCode,
    questions: s.questions.map(mapQuestionDtoToUI),
  };
}

export function mapSnapshotUIToDto(
  s: SnapshotUI,
  quizId: string,
): SnapshotCreateRequestDto {
  return {
    quizId: quizId,
    title: s.title,
    description: s.description,
    timeLimit: s.timeLimit,
    pinCode: s.pinCode,
    questions: s.questions.map(mapQuestionUIToDto),
  };
}
