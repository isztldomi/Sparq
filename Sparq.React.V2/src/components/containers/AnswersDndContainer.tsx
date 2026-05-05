import {
  DndContext,
  PointerSensor,
  TouchSensor,
  useSensor,
  useSensors,
  closestCenter,
} from "@dnd-kit/core";

import {
  SortableContext,
  rectSortingStrategy,
  arrayMove,
} from "@dnd-kit/sortable";

import { useState } from "react";
import { SortableAnswerContainer } from "./SortableAnswerContainer";
import type { AnswerUI } from "@/features/answer/answerTypes";

type AnswersDndContainerProp = {
  answers: AnswerUI[];
  onReorderAnswer: (answers: AnswerUI[]) => void;
  onUpdateAnswer: (answerId: string, field: keyof AnswerUI, value: any) => void;
  onDeleteAnswer: (answerId: string) => void;
};

export function AnswersDndContainer({
  answers,
  onReorderAnswer,
  onUpdateAnswer,
  onDeleteAnswer,
}: AnswersDndContainerProp) {
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: { distance: 8 },
    }),
    useSensor(TouchSensor, {
      activationConstraint: { delay: 150, tolerance: 5 },
    }),
  );

  const [activeId, setActiveId] = useState<string | null>(null);

  function handleDragStart(event: any) {
    setActiveId(event.active.id);
  }

  function handleDragCancel() {
    setActiveId(null);
  }

  function handleDragEnd(event: any) {
    const { active, over } = event;
    setActiveId(null);

    if (!over || active.id === over.id) return;

    const oldIndex = answers.findIndex((a) => a.id === active.id);
    const newIndex = answers.findIndex((a) => a.id === over.id);

    onReorderAnswer(arrayMove(answers, oldIndex, newIndex));
  }

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragStart={handleDragStart}
      onDragEnd={handleDragEnd}
      onDragCancel={handleDragCancel}
    >
      <SortableContext
        items={answers.map((a) => a.id)}
        strategy={rectSortingStrategy}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-2 auto-rows-fr touch-none">
          {answers.map((a) => (
            <SortableAnswerContainer
              key={a.id}
              answer={a}
              onUpdate={(field, value) => onUpdateAnswer(a.id, field, value)}
              onDelete={() => onDeleteAnswer(a.id)}
              isActive={activeId === a.id}
            />
          ))}
        </div>
      </SortableContext>
    </DndContext>
  );
}
