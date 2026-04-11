import type { QuizCreateRequestDto } from "@/api/models/quizDto/QuizCreateRequestDto";

type QuestionCreateContainerProps = {
  questions: QuizCreateRequestDto[];
  updateQuestion: () => void;
  addAnswer: () => void;
  updateAnswer: () => void;
};

export function QuestionCreateContainer({
  questions,
  updateQuestion,
  addAnswer,
  updateAnswer,
}: QuestionCreateContainerProps) {
  return <></>;
}
