import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { Grip } from "lucide-react";

import type { QuestionUI } from "@/features/question/questionTypes";
import { SortableQuestionCard } from "@/components/cards/SortableQuestionCard";
import { ToggleButton } from "@/components/buttons/toggleButton";
import type { AnswerUI } from "@/features/answer/answerTypes";

type SortableQuestionContainerProps = {
  question: QuestionUI;
  indexQuestion: number;
  totalQuestion: number;
  onToggle: () => void;
  onDeleteQuestion: () => void;
  onUpdateQuestion: (field: keyof QuestionUI, value: any) => void;
  onAddAnswer: () => void;
  onUpdateAnswer: (answerId: string, field: keyof AnswerUI, value: any) => void;
  onDeleteAnswer: (answerId: string) => void;
  draggingIdQuestion?: string | null;
  getClientError: (path: string) => string | undefined;
};

export function SortableQuestionContainer({
  question,
  indexQuestion,
  totalQuestion,
  onToggle,
  onDeleteQuestion,
  onUpdateQuestion,
  onAddAnswer,
  onUpdateAnswer,
  onDeleteAnswer,
  draggingIdQuestion,
  getClientError,
}: SortableQuestionContainerProps) {
  const isDragging = draggingIdQuestion === question.id;

  const { attributes, listeners, setNodeRef, transform, transition } =
    useSortable({
      id: question.id,
    });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      className={`
        bg-[var(--surface-4)] rounded-lg pl-1 p-4 shadow-sm
        transition-all duration-200
        ${isDragging ? "opacity-40 scale-[0.98]" : ""}
      `}
    >
      {/* ROW */}
      <div className="flex items-stretch gap-3 flex-wrap">
        {/* DRAG */}
        <div
          {...attributes}
          {...listeners}
          className="cursor-grab active:cursor-grabbing shrink-0 touch-none"
        >
          <Grip className="w-10 h-10 text-[var(--text-muted)] hover:text-[var(--text-h)] transition" />
        </div>

        {/* INDEX */}
        <div className="flex justify-center text-sm text-[var(--text-h)] bg-[var(--surface-5)] p-2 rounded-lg whitespace-nowrap">
          {indexQuestion + 1} / {totalQuestion}
        </div>

        {/* TITLE INPUT */}
        <input
          value={question.title}
          onChange={(e) => onUpdateQuestion("title", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          placeholder={`${indexQuestion + 1} Question Title`}
          className={`flex-1 min-w-[150px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] outline-none border
            ${
              getClientError(`snapshots.0.questions.${indexQuestion}.title`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
        />

        {/* TIMELIMIT */}
        <div
          className={`flex min-w-[50px] max-w-[110px] p-2 rounded-lg bg-[var(--surface-5)] border
            ${
              getClientError(`snapshots.0.questions.${indexQuestion}.timeLimit`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
        >
          <input
            type="number"
            value={question.timeLimit}
            onChange={(e) =>
              onUpdateQuestion("timeLimit", Number(e.target.value))
            }
            onPointerDown={(e) => e.stopPropagation()}
            className="w-full text-right outline-none text-[var(--text-h)]"
          />
          <span className="text-[var(--text-h)] text-[var(--text-h)]">s</span>
        </div>

        {/* POINT */}
        <div
          className={`flex min-w-[50px] max-w-[110px] p-2 rounded-lg bg-[var(--surface-5)] border
            ${
              getClientError(`snapshots.0.questions.${indexQuestion}.point`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
        >
          <input
            type="number"
            value={question.point}
            onChange={(e) => onUpdateQuestion("point", Number(e.target.value))}
            onPointerDown={(e) => e.stopPropagation()}
            className="w-full text-right outline-none text-[var(--text-h)]"
          />
          <span className="text-[var(--text-h)] text-[var(--text-h)]">p</span>
        </div>

        {/* TOGGLE */}
        <div className="flex items-center p-2 rounded-lg bg-[var(--surface-5)]">
          <ToggleButton isOpen={question.isOpen} onClick={onToggle} />
        </div>
      </div>

      {/* EXPANDED SECTION */}
      <div
        className={`
          overflow-hidden transition-all duration-300 ease-in-out
          ${
            question.isOpen && !isDragging
              ? "opacity-100 mt-4"
              : "max-h-0 opacity-0 mt-0"
          }
        `}
      >
        <SortableQuestionCard
          question={question}
          indexQuestion={indexQuestion}
          totalQuestion={totalQuestion}
          onDeleteQuestion={onDeleteQuestion}
          onUpdateQuestion={onUpdateQuestion}
          onAddAnswer={onAddAnswer}
          onUpdateAnswer={onUpdateAnswer}
          onDeleteAnswer={onDeleteAnswer}
          getClientError={getClientError}
        />
      </div>
    </div>
  );
}
