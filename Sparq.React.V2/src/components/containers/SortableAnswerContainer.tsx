import type { AnswerUI } from "@/features/answer/answerTypes";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { GripVertical } from "lucide-react";
import { GreenRedCheckbox } from "../checkbox/greenRedCheckbox";
import { RedButton } from "../buttons/redButton";

type SortableAnswerContainerProps = {
  answer: AnswerUI;
  indexQuestion: number;
  indexAnswer: number;
  onUpdate: (field: keyof AnswerUI, value: any) => void;
  onDelete: () => void;
  isActive?: boolean;
  getClientError: (path: string) => string | undefined;
};
export function SortableAnswerContainer({
  answer,
  indexQuestion,
  indexAnswer,
  onUpdate,
  onDelete,
  getClientError,
}: SortableAnswerContainerProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: answer.id });

  return (
    <div
      ref={setNodeRef}
      style={{
        transform: CSS.Transform.toString(transform),
        transition,
        zIndex: isDragging ? 50 : undefined,
        opacity: isDragging ? 0.5 : 1,
      }}
      className="flex gap-2 items-center bg-[var(--surface-5)] pl-0 pr-6 p-2 rounded-lg touch-none"
    >
      <div {...attributes} {...listeners} className="cursor-grab">
        <GripVertical className="w-10 h-10 text-[var(--text-muted)] hover:text-[var(--text-h)] transition" />
      </div>
      <div className="flex flex-col w-full gap-4">
        <div className="bg-[var(--surface-6)] w-full p-3 rounded-lg">
          <textarea
            value={answer.text}
            onChange={(e) => onUpdate("text", e.target.value)}
            className={`bg-transparent outline-none w-full resize-none rounded-lg p-2 border
            ${
              getClientError(
                `snapshots.0.questions.${indexQuestion}.answers.${indexAnswer}.text`,
              )
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
          />
        </div>
        <div className="flex justify-between items-center w-full gap-4 h-10">
          <GreenRedCheckbox
            value={answer.isCorrect}
            onChange={(val) => onUpdate("isCorrect", val)}
            trueLabel="Correct"
            falseLabel="Wrong"
            className={`flex-1 h-full border
            ${
              getClientError(`snapshots.0.questions.${indexQuestion}.answers`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
          ></GreenRedCheckbox>
          <RedButton onClick={onDelete} className="flex-1 h-full">
            Remove
          </RedButton>
        </div>
      </div>
    </div>
  );
}
