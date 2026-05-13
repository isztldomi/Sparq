export interface AnswerResponseDto {
  id: string;
  text: string;
  isCorrect: boolean;
  order: number;
}

export interface AnswerCreateRequestDto {
  text: string;
  isCorrect: boolean;
  order: number;
}

export type AnswerUI = AnswerCreateRequestDto & {
  id: string;
};

export type CurrentQuestionAnswerWithoutResultDto = {
  id: string;
  text: string;
  order: number;
};

export type CurrentQuestionAnswerWithResultDto = {
  id: string;
  text: string;
  isCorrect: boolean;
  order: number;
};
