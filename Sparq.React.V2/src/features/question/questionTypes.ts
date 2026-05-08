import type {
  AnswerResponseDto,
  AnswerCreateRequestDto,
  AnswerUI,
} from "@/features/answer/answerTypes";

export type QuestionResponseDto = {
  id: string;
  title: string;
  text: string;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: AnswerResponseDto[];
};

export type QuestionCreateRequestDto = {
  title: string;
  text: string;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: AnswerCreateRequestDto[];
};

export type QuestionUI = Omit<QuestionCreateRequestDto, "answers"> & {
  id: string;
  isOpen: boolean;
  mediaFile: File | null;
  mediaPreviewUrl: string | null;
  answers: AnswerUI[];
};
