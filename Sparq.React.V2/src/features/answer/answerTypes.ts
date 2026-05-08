export interface AnswerResponseDto {
  id: string;
  text: string;
  isCorrect: boolean;
}

export interface AnswerCreateRequestDto {
  text: string;
  isCorrect: boolean;
}

export type AnswerUI = AnswerCreateRequestDto & {
  id: string;
};
