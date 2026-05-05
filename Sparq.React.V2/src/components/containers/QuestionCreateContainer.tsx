import type { QuestionUI } from "@/features/question/questionTypes";
import { SortableQuestion } from "@/components/containers/SortableQuestionContainer";

type QuestionCreateContainerProps = {
  questions: QuestionUI[];
  onAddQuestion: () => void;
  onUpdateQuestion: (index: number, field: string, value: any) => void;
  onRemoveQuestion: (index: number) => void;
};

export function QuestionCreateContainer({
  questions,
  onAddQuestion,
  onUpdateQuestion,
  onRemoveQuestion,
}: QuestionCreateContainerProps) {
  return (
    <div className="flex flex-col gap-2">
      {questions.map((q, index) => (
        <SortableQuestion
          key={q.id}
          question={q}
          index={index}
          onUpdate={onUpdateQuestion}
          onRemove={onRemoveQuestion}
        />
      ))}

      <button onClick={onAddQuestion} className="mt-2">
        + Add question
      </button>
    </div>
  );
}
