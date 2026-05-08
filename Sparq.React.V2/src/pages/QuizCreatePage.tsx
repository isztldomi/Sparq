import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import { GreenButton } from "@/components/buttons/greenButton";

import type { SnapshotUI } from "@/features/snapshot/snapshotTypes";
import type { QuizUI } from "@/features/quiz/quizTypes";
import type { QuestionUI } from "@/features/question/questionTypes";

import { QuestionsDndContainer } from "@/components/containers/QuestionsDndContainer";
import { arrayMove } from "@dnd-kit/sortable";
import type { AnswerUI } from "@/features/answer/answerTypes";
import { GreenRedCheckbox } from "@/components/checkbox/greenRedCheckbox";
import { useUploadMediaMutation } from "@/features/media/mediaApi";
import { useCreateQuizMutation } from "@/features/quiz/quizApi";
import { mapQuizUIToDto } from "@/features/quiz/quizMapper";
import type { ProblemDetails } from "@/api/models/ProblemDetails";
import { flattenErrors } from "@/api/core/flattenErrors";
import { quizSchema } from "@/schemas/quiz/quizSchema";
import { buildErrorMap } from "@/utils/clientErrors/buildErrorMap";
import { v4 as uuidv4 } from "uuid";

export function QuizCreatePage() {
  const navigate = useNavigate();
  const [uploadMedia] = useUploadMediaMutation();
  const [createQuiz] = useCreateQuizMutation();

  const [serverErrors, setServerErrors] = useState<
    { field: string; message: string }[]
  >([]);

  const [formErrors, setFormErrors] = useState<Record<string, string>>({});

  const initialSnapshot: SnapshotUI = {
    title: "",
    description: "",
    timeLimit: 10,
    pinCode: "",
    questions: [],
  };

  const [formData, setFormData] = useState<QuizUI>({
    isPublic: false,
    snapshots: [initialSnapshot],
  });

  const snapshot = formData.snapshots[0];

  function clamp(value: number, min: number, max: number) {
    return Math.max(min, Math.min(max, value));
  }

  function updateSnapshot(field: keyof SnapshotUI, value: any) {
    setFormData((prev) => {
      let newValue = value;

      if (field === "timeLimit") {
        const parsed = Number(value);

        if (!isNaN(parsed)) {
          newValue = clamp(Number(value), 10, 7200);
        } else {
          return prev;
        }
      }

      return {
        ...prev,
        snapshots: [
          {
            ...prev.snapshots[0],
            [field]: newValue,
          },
        ],
      };
    });
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
      const questions = prev.snapshots[0].questions.map((q) => {
        if (q.id !== id) return q;

        let newValue = value;

        if (field === "timeLimit") {
          newValue = clamp(Number(value), 10, 7200);
        }

        if (field === "point") {
          newValue = clamp(Number(value), 0, 10);
        }

        return { ...q, [field]: newValue };
      });

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
        id: uuidv4(),
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
          if (a.id === answerId) {
            return { ...a, [field]: value };
          }

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
          id: uuidv4(),
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

  // ---------------------------
  // DONE
  // ---------------------------
  async function handleDone() {
    const snapshot = formData.snapshots[0];

    // 1. ORDER FIX (QUESTION + ANSWER)
    const orderedQuestions = snapshot.questions.map((q, qIndex) => {
      const orderedAnswers = q.answers.map((a, aIndex) => ({
        ...a,
        order: aIndex,
      }));

      return {
        ...q,
        order: qIndex,
        answers: orderedAnswers,
      };
    });

    // VALIDATION
    const result = quizSchema.safeParse({
      ...formData,
      snapshots: [
        {
          ...snapshot,
          questions: orderedQuestions,
        },
      ],
    });

    if (!result.success) {
      const errorMap = buildErrorMap(result.error.issues);
      setFormErrors(errorMap);
      return;
    }

    setFormErrors({});

    try {
      // MEDIA UPLOAD
      const questionsWithMedia = await Promise.all(
        orderedQuestions.map(async (q) => {
          if (!q.mediaFile) return q;

          const res = await uploadMedia(q.mediaFile).unwrap();

          return {
            ...q,
            mediaId: res.id,
          };
        }),
      );

      const finalFormData = {
        ...formData,
        snapshots: [
          {
            ...snapshot,
            questions: questionsWithMedia,
          },
        ],
      } as QuizUI;

      const dto = mapQuizUIToDto(finalFormData);

      await createQuiz(dto).unwrap();
      navigate(`/my-quizzes`);
    } catch (err: unknown) {
      const error = err as { data?: ProblemDetails };
      setServerErrors(flattenErrors(error.data?.errors));
    }
  }

  // ---------------------------
  // FRONTEND ERRORS
  // ---------------------------
  function getClientError(path: string) {
    return formErrors[path];
  }

  return (
    <div className="min-h-screen p-4 flex flex-col gap-6">
      {/* HEADER */}
      <div className="flex justify-between items-center">
        <GreenButton className="w-30 h-10" onClick={() => navigate(-1)}>
          Back
        </GreenButton>
        <h1 className="text-xl">Quiz Create</h1>

        <GreenButton className="w-30 h-10" onClick={handleDone}>
          Save
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
          <div
            className={`flex items-center w-50 p-2 rounded-lg bg-[var(--surface-5)] border
                          ${
                            getClientError(`snapshots.0.timeLimit`)
                              ? "border-[var(--error-text)]"
                              : "border-transparent"
                          }`}
          >
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
          <div>
            <div
              className={`flex items-center w-50 p-2 rounded-lg bg-[var(--surface-5)] border
                          ${
                            getClientError(`snapshots.0.pinCode`)
                              ? "border-[var(--error-text)]"
                              : "border-transparent"
                          }`}
            >
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
        </div>

        {/* TITLE INPUT */}
        <div>
          <input
            value={snapshot.title}
            onChange={(e) => updateSnapshot("title", e.target.value)}
            onPointerDown={(e) => e.stopPropagation()}
            placeholder="Quiz Title"
            className={`flex-1 w-full min-w-[150px] p-2 rounded-lg bg-[var(--surface-5)] text-[var(--text-h)] outline-none border
            ${
              getClientError(`snapshots.0.title`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
          />
        </div>

        <div>
          <textarea
            placeholder="Quiz Description"
            value={snapshot.description}
            onChange={(e) => updateSnapshot("description", e.target.value)}
            onPointerDown={(e) => e.stopPropagation()}
            className={`w-full min-h-[80px] bg-[var(--surface-5)] text-[var(--text-h)] rounded-lg p-2 outline-none border
            ${
              getClientError(`snapshots.0.description`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
          />
        </div>
      </div>

      {getClientError("snapshots.0.questions") && (
        <p className="text-[var(--error-text)]">
          {getClientError("snapshots.0.questions")}
        </p>
      )}
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
        getClientError={getClientError}
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
