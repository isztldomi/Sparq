import { useState } from "react";

import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import { GreenButton } from "@/components/buttons/greenButton";

import type { SnapshotUI } from "@/features/snapshot/snapshotTypes";
import type { QuizUI } from "@/features/quiz/quizTypes";
import type { QuestionUI } from "@/features/question/questionTypes";

import { QuestionsDndContainer } from "@/components/containers/QuestionsDndContainer";
import { arrayMove } from "@dnd-kit/sortable";

export function QuizCreatePage() {
  const [serverErrors] = useState<{ field: string; message: string }[]>([]);

  const initialSnapshot: SnapshotUI = {
    title: "",
    description: "",
    timeLimit: 10,
    pinCode: "",
    questions: [],
  };

  const [nextId, setNextId] = useState(1);
  const [formData, setFormData] = useState<QuizUI>({
    isPublic: false,
    snapshots: [initialSnapshot],
  });

  const snapshot = formData.snapshots[0];

  // ---------------------------
  // SNAPSHOT UPDATE
  // ---------------------------
  function updateQuestions(questions: QuestionUI[]) {
    setFormData((prev) => ({
      ...prev,
      snapshots: [
        {
          ...prev.snapshots[0],
          questions,
        },
      ],
    }));
  }

  function updateQuestion(id: string, field: string, value: any) {
    setFormData((prev) => {
      const questions = prev.snapshots[0].questions.map((q) =>
        q.id === id ? { ...q, [field]: value } : q,
      );

      return {
        ...prev,
        snapshots: [
          {
            ...prev.snapshots[0],
            questions,
          },
        ],
      };
    });
  }

  // ---------------------------
  // ADD QUESTION
  //  alert(snapshot.questions.length);
  //  console.log(snapshot.questions.length);
  // ---------------------------
  function addQuestion() {
    setFormData((prev) => {
      const newQuestion: QuestionUI = {
        id: String(nextId),
        isOpen: false,
        title: "",
        text: "",
        mediaUrl: "",
        timeLimit: 10,
        point: 0,
        answers: [],
      };

      return {
        ...prev,
        snapshots: [
          {
            ...prev.snapshots[0],
            questions: [...prev.snapshots[0].questions, newQuestion],
          },
        ],
      };
    });

    setNextId((prev) => prev + 1);
  }

  function removeQuestion(id: string) {
    setFormData((prev) => ({
      ...prev,
      snapshots: [
        {
          ...prev.snapshots[0],
          questions: prev.snapshots[0].questions.filter((q) => q.id !== id),
        },
      ],
    }));
  }

  // ---------------------------
  // TOGGLE OPEN
  // ---------------------------
  function toggleOpen(id: string) {
    setFormData((prev) => ({
      ...prev,
      snapshots: [
        {
          ...prev.snapshots[0],
          questions: prev.snapshots[0].questions.map((q) =>
            q.id === id ? { ...q, isOpen: !q.isOpen } : q,
          ),
        },
      ],
    }));
  }

  // ---------------------------
  // DRAG END
  // ---------------------------
  function handleDragEnd(event: any) {
    const { active, over } = event;

    if (!over || active.id === over.id) return;

    setFormData((prev) => {
      const questions = prev.snapshots[0].questions;

      const oldIndex = questions.findIndex((q) => q.id === active.id);
      const newIndex = questions.findIndex((q) => q.id === over.id);

      return {
        ...prev,
        snapshots: [
          {
            ...prev.snapshots[0],
            questions: arrayMove(questions, oldIndex, newIndex),
          },
        ],
      };
    });
  }

  return (
    <div className="min-h-screen p-4 flex flex-col gap-6">
      {/* HEADER */}
      <div className="flex justify-between items-center">
        <h1 className="text-xl">Quiz Create</h1>

        <GreenButton className="w-30 h-10">Done</GreenButton>
      </div>

      <ErrorsContainer serverErrors={serverErrors} />

      {/* QUESTIONS */}
      <QuestionsDndContainer
        questions={snapshot.questions}
        onDragEnd={handleDragEnd}
        onToggle={toggleOpen}
        onDelete={removeQuestion}
        onUpdate={updateQuestion}
      />

      {/* ADD */}
      <div className="flex justify-center relative z-50">
        <GreenButton onClick={addQuestion} className="w-40 h-10">
          + Add question
        </GreenButton>
      </div>
    </div>
  );
}
