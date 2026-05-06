import type {
  AnswerResponseDto,
  AnswerCreateRequestDto,
  AnswerUI,
} from "@/features/answer/answerTypes";

export interface QuestionResponseDto {
  id: number;
  title: string;
  text: string;
  mediaId: string;
  point: number;
  answers: AnswerResponseDto[];
}

export interface QuestionCreateRequestDto {
  title: string;
  text: string;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: AnswerCreateRequestDto[];
}

export type QuestionUI = QuestionCreateRequestDto & {
  id: string;
  isOpen: boolean;
  answers: AnswerUI[];
  mediaFile: File | null;
  mediaPreviewUrl: string | null;
};
