import {
  DndContext,
  closestCenter,
  PointerSensor,
  useSensor,
  useSensors,
} from "@dnd-kit/core";

import {
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";

import type { QuestionUI } from "@/features/question/questionTypes";
import { SortableQuestionContainer } from "@/components/containers/SortableQuestionContainer";

type QuestionsDndContainerProps = {
  questions: QuestionUI[];
  onDragEnd: (event: any) => void;
  onToggle: (id: string) => void;
  onDelete: (id: string) => void;
  onUpdate: (id: string, field: string, value: any) => void;
};

export function QuestionsDndContainer({
  questions,
  onDragEnd,
  onToggle,
  onDelete,
  onUpdate,
}: QuestionsDndContainerProps) {
  // 🔥 MOBIL FIX: sensor
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8, // mobilon is csak mozdítás után indul drag
      },
    }),
  );

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragEnd={onDragEnd}
    >
      <SortableContext
        items={questions.map((q) => q.id)}
        strategy={verticalListSortingStrategy}
      >
        <div className="flex flex-col gap-2">
          {questions.map((q, index) => (
            <SortableQuestionContainer
              key={q.id}
              question={q}
              index={index}
              total={questions.length}
              onToggle={() => onToggle(q.id)}
              onDelete={() => onDelete(q.id)}
              onUpdate={(field, value) => onUpdate(q.id, field, value)}
            />
          ))}
        </div>
      </SortableContext>
    </DndContext>
  );
}
