import { useState } from "react";
import { DndContext, closestCenter } from "@dnd-kit/core";
import {
  SortableContext,
  arrayMove,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";

import { SortableQuestion } from "@/components/containers/SortableQuestionContainer";

type Question = {
  id: string;
  title: string;
  isOpen: boolean;
};

export function QuizCreatePage() {
  const [questions, setQuestions] = useState<Question[]>([]);

  function addQuestion() {
    setQuestions((prev) => [
      ...prev,
      {
        id: crypto.randomUUID(),
        title: `Question ${prev.length + 1}`,
        isOpen: false,
      },
    ]);
  }

  function toggleOpen(id: string) {
    setQuestions((prev) =>
      prev.map((q) => (q.id === id ? { ...q, isOpen: !q.isOpen } : q)),
    );
  }

  function handleDragEnd(event: any) {
    const { active, over } = event;

    if (!over || active.id === over.id) return;

    setQuestions((prev) => {
      const oldIndex = prev.findIndex((q) => q.id === active.id);
      const newIndex = prev.findIndex((q) => q.id === over.id);

      return arrayMove(prev, oldIndex, newIndex);
    });
  }

  return (
    <div className="p-4">
      <button onClick={addQuestion} className="mb-4">
        + Add question
      </button>

      <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
        <SortableContext
          items={questions.map((q) => q.id)}
          strategy={verticalListSortingStrategy}
        >
          <div className="flex flex-col gap-2">
            {questions.map((q, index) => (
              <SortableQuestion
                key={q.id}
                question={q}
                index={index}
                total={questions.length}
                onToggle={() => toggleOpen(q.id)}
              />
            ))}
          </div>
        </SortableContext>
      </DndContext>
    </div>
  );
}
