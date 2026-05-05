import { useState } from "react";

import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import { GreenButton } from "@/components/buttons/greenButton";

import type { SnapshotUI } from "@/features/snapshot/snapshotTypes";
import type { QuizUI } from "@/features/quiz/quizTypes";
import type { QuestionUI } from "@/features/question/questionTypes";

import { QuestionsDndContainer } from "@/components/containers/QuestionsDndContainer";
import { arrayMove } from "@dnd-kit/sortable";
import type { AnswerUI } from "@/features/answer/answerTypes";
import { GreenRedCheckbox } from "@/components/checkbox/greenRedCheckbox";

export function QuizCreatePage() {
  const [serverErrors] = useState<{ field: string; message: string }[]>([]);

  const initialSnapshot: SnapshotUI = {
    title: "",
    description: "",
    timeLimit: 10,
    pinCode: "",
    questions: [],
  };

  const [nextIdQuestion, setNextIdQuestion] = useState(1);
  const [nextIdAnswer, setNextIdAnswer] = useState(1);
  const [formData, setFormData] = useState<QuizUI>({
    isPublic: false,
    snapshots: [initialSnapshot],
  });

  const snapshot = formData.snapshots[0];

  function updateSnapshot(field: keyof SnapshotUI, value: any) {
    setFormData((prev) => ({
      ...prev,
      snapshots: [
        {
          ...prev.snapshots[0],
          [field]: value,
        },
      ],
    }));
  }

  // ---------------------------
  // QUESTION
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

  function addQuestion() {
    setFormData((prev) => {
      const newQuestion: QuestionUI = {
        id: String(nextIdQuestion),
        isOpen: false,
        title: "",
        text: "",
        timeLimit: 10,
        point: 0,
        answers: [],
        mediaFile: null,
        mediaPreviewUrl: null,
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

    setNextIdQuestion((prev) => prev + 1);
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
  // ANSWER
  // ---------------------------
  function updateAnswers(questionId: string, answers: AnswerUI[]) {
    setFormData((prev) => {
      const questions = prev.snapshots[0].questions.map((q) =>
        q.id === questionId ? { ...q, answers } : q,
      );

      return {
        ...prev,
        snapshots: [{ ...prev.snapshots[0], questions }],
      };
    });
  }

  function updateAnswer(
    questionId: string,
    answerId: string,
    field: keyof AnswerUI,
    value: any,
  ) {
    setFormData((prev) => {
      const questions = prev.snapshots[0].questions.map((q) => {
        if (q.id !== questionId) return q;

        const answers = q.answers.map((a) => {
          // ha ez az aktuális answer
          if (a.id === answerId) {
            return { ...a, [field]: value };
          }

          // 👇 EZ A LÉNYEG
          // ha isCorrect-et állítunk true-ra, minden más legyen false
          if (field === "isCorrect" && value === true) {
            return { ...a, isCorrect: false };
          }

          return a;
        });

        return { ...q, answers };
      });

      return {
        ...prev,
        snapshots: [{ ...prev.snapshots[0], questions }],
      };
    });
  }

  function addAnswer(questionId: string) {
    setFormData((prev) => {
      const questions = prev.snapshots[0].questions.map((q) => {
        if (q.id !== questionId) return q;

        const newAnswer: AnswerUI = {
          id: String(nextIdAnswer),
          text: "",
          isCorrect: false,
        };

        return { ...q, answers: [...q.answers, newAnswer] };
      });

      return {
        ...prev,
        snapshots: [{ ...prev.snapshots[0], questions }],
      };
    });
    setNextIdAnswer((prev) => prev + 1);
  }

  function removeAnswer(questionId: string, answerId: string) {
    setFormData((prev) => {
      const questions = prev.snapshots[0].questions.map((q) => {
        if (q.id !== questionId) return q;

        return {
          ...q,
          answers: q.answers.filter((a) => a.id !== answerId),
        };
      });

      return {
        ...prev,
        snapshots: [{ ...prev.snapshots[0], questions }],
      };
    });
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
  function handleDragEndQuestion(event: any) {
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

        <GreenButton
          className="w-30 h-10"
          onClick={() => {
            console.log("FORM DATA:", structuredClone(formData));
          }}
        >
          Done
        </GreenButton>
      </div>
      <ErrorsContainer serverErrors={serverErrors} />

      <div className="flex flex-col gap-2 bg-[var(--surface-4)] p-2 rounded-lg">
        <div className="flex flex-wrap gap-x-4 gap-y-2 w-full">
          <GreenRedCheckbox
            value={formData.isPublic}
            onChange={(value) =>
              setFormData((prev) => ({
                ...prev,
                isPublic: value,
              }))
            }
            trueLabel="Public"
            falseLabel="Private"
            className="w-30 h-10"
          />
          {/* TIMELIMIT */}
          <div className="flex items-center w-50 p-2 rounded-lg bg-[var(--surface-5)]">
            <div className="whitespace-pre">Time Limit:</div>
            <input
              type="number"
              value={snapshot.timeLimit}
              onChange={(e) => updateSnapshot("timeLimit", e.target.value)}
              onPointerDown={(e) => e.stopPropagation()}
              className="w-full text-right outline-none text-[var(--text-h)]"
            />
            <span className="text-[var(--text-h)] text-[var(--text-h)]">s</span>
          </div>

          {/* PINCODE */}
          <div className="flex items-center w-50 p-2 rounded-lg bg-[var(--surface-5)]">
            <div className="whitespace-pre">Pin Code:</div>

            <input
              type="text"
              inputMode="numeric"
              pattern="[0-9]*"
              value={snapshot.pinCode}
              onChange={(e) => updateSnapshot("pinCode", e.target.value)}
              onPointerDown={(e) => e.stopPropagation()}
              className="w-full text-right outline-none text-[var(--text-h)] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none [-moz-appearance:textfield]"
            />
          </div>
        </div>

        {/* TITLE INPUT */}
        <input
          value={snapshot.title}
          onChange={(e) => updateSnapshot("title", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          placeholder="Quiz Title"
          className="flex-1 min-w-[150px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] outline-none"
        />

        <textarea
          placeholder="Quiz Description"
          value={snapshot.description}
          onChange={(e) => updateSnapshot("description", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          className="
            w-full 
            min-h-[80px]
            bg-[var(--surface-5)] 
            text-[var(--text-h)]
            border border-transparent
            rounded-lg 
            p-2 
            outline-none
          "
        />
      </div>

      {/* QUESTIONS */}
      <QuestionsDndContainer
        questions={snapshot.questions}
        onDragEndQuestion={handleDragEndQuestion}
        onToggle={toggleOpen}
        onDeleteQuestion={removeQuestion}
        onUpdateQuestion={updateQuestion}
        onAddAnswer={addAnswer}
        onUpdateAnswer={updateAnswer}
        onDeleteAnswer={removeAnswer}
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
