import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { Grip } from "lucide-react";

import type { QuestionUI } from "@/features/question/questionTypes";
import { SortableQuestionCard } from "@/components/cards/SortableQuestionCard";
import { ToggleButton } from "@/components/buttons/toggleButton";

type SortableQuestionContainerProps = {
  question: QuestionUI;
  index: number;
  total: number;
  onToggle: () => void;
  onDelete: () => void;
  onUpdate: (field: keyof QuestionUI, value: any) => void;
};

export function SortableQuestionContainer({
  question,
  index,
  total,
  onToggle,
  onDelete,
  onUpdate,
}: SortableQuestionContainerProps) {
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
      className="bg-[var(--surface-4)] rounded-lg pl-1 p-4 shadow-sm"
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
          {index + 1} / {total}
        </div>

        {/* TITLE INPUT */}
        <input
          value={question.title}
          onChange={(e) => onUpdate("title", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          placeholder={`${index + 1} Question Title`}
          className="flex-1 min-w-[150px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] outline-none"
        />

        {/* TIMELIMIT INPUT */}
        <input
          type="number"
          value={question.timeLimit}
          onChange={(e) => onUpdate("timeLimit", Number(e.target.value))}
          onPointerDown={(e) => e.stopPropagation()}
          className="flex-1 min-w-[50px] max-w-[110px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] text-center outline-none"
        />

        {/* POINT INPUT */}
        <input
          type="number"
          value={question.point}
          onChange={(e) => onUpdate("point", Number(e.target.value))}
          onPointerDown={(e) => e.stopPropagation()}
          className="flex-1 min-w-[50px] max-w-[70px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] text-center outline-none"
        />

        {/* TOGGLE */}
        <div className="flex items-center p-2 rounded-lg bg-[var(--surface-5)]">
          <ToggleButton isOpen={question.isOpen} onClick={onToggle} />
        </div>
      </div>

      {/* EXPANDED */}
      <div
        className={`
          overflow-hidden transition-all duration-300 ease-in-out
          ${question.isOpen ? "max-h-[500px] opacity-100 mt-4" : "max-h-0 opacity-0 mt-0"}
        `}
      >
        <SortableQuestionCard
          question={question}
          index={index}
          total={total}
          onDelete={onDelete}
          onUpdate={onUpdate}
        />
      </div>
    </div>
  );
}
