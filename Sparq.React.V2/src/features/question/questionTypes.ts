import type {
  AnswerResponseDto,
  AnswerCreateRequestDto,
  AnswerUI,
  CurrentQuestionAnswerWithoutResultDto,
  CurrentQuestionAnswerWithResultDto,
} from "@/features/answer/answerTypes";

export type QuestionResponseDto = {
  id: string;
  title: string;
  text: string;
  order: number;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: AnswerResponseDto[];
};

export type QuestionCreateRequestDto = {
  title: string;
  text: string;
  order: number;
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

export type CurrentQuestionWithoutResultDto = {
  id: string;
  title: string;
  text: string;
  order: number;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: CurrentQuestionAnswerWithoutResultDto[];
};

export type CurrentQuestionWithResultDto = {
  id: string;
  title: string;
  text: string;
  order: number;
  mediaId: string | null;
  timeLimit: number;
  point: number;
  answers: CurrentQuestionAnswerWithResultDto[];
};

export type CurrentSessionQuestionStateWithoutResultDto = {
  id: string;
  question: CurrentQuestionWithoutResultDto;
  order: number;
  startedAt: string | null;
  endsAt: string | null;
};

export type CurrentSessionQuestionStateWithResultDto = {
  id: string;
  question: CurrentQuestionWithResultDto;
  order: number;
  startedAt: string | null;
  endsAt: string | null;
  isActive: boolean;
};
