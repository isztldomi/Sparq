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
