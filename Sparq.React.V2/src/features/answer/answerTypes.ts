export interface AnswerResponseDto {
  id: number;
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
