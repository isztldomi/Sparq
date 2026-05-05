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
import { useState } from "react";
import type { AnswerUI } from "@/features/answer/answerTypes";

type QuestionsDndContainerProps = {
  questions: QuestionUI[];
  onDragEndQuestion: (event: any) => void;
  onToggle: (id: string) => void;
  onDeleteQuestion: (id: string) => void;
  onUpdateQuestion: (id: string, field: string, value: any) => void;
  onAddAnswer: (questionId: string) => void;
  onUpdateAnswer: (
    questionId: string,
    answerId: string,
    field: keyof AnswerUI,
    value: any,
  ) => void;
  onDeleteAnswer: (questionId: string, answerId: string) => void;
};

export function QuestionsDndContainer({
  questions,
  onDragEndQuestion,
  onToggle,
  onDeleteQuestion,
  onUpdateQuestion,
  onAddAnswer,
  onUpdateAnswer,
  onDeleteAnswer,
}: QuestionsDndContainerProps) {
  const [draggingId, setDraggingId] = useState<string | null>(null);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    }),
  );

  function handleDragStart(event: any) {
    setDraggingId(event.active.id);
  }

  function handleDragEnd(event: any) {
    setDraggingId(null);
    onDragEndQuestion(event);
  }

  function handleDragCancel() {
    setDraggingId(null);
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
        items={questions.map((q) => q.id)}
        strategy={verticalListSortingStrategy}
      >
        <div className="flex flex-col gap-2">
          {questions.map((q, index) => (
            <SortableQuestionContainer
              key={q.id}
              question={q}
              indexQuestion={index}
              totalQuestion={questions.length}
              onToggle={() => onToggle(q.id)}
              onDeleteQuestion={() => onDeleteQuestion(q.id)}
              onUpdateQuestion={(field, value) =>
                onUpdateQuestion(q.id, field, value)
              }
              onAddAnswer={() => onAddAnswer(q.id)}
              onUpdateAnswer={(answerId, field, value) =>
                onUpdateAnswer(q.id, answerId, field, value)
              }
              onDeleteAnswer={(answerId) => onDeleteAnswer(q.id, answerId)}
              draggingIdQuestion={draggingId}
            />
          ))}
        </div>
      </SortableContext>
    </DndContext>
  );
}
