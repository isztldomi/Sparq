import { useState } from "react";
import { createQuiz } from "@/api/client/quizzes-client";
import type { QuizCreateRequestDto } from "@/api/models/quizDto/QuizCreateRequestDto";
//import { useNavigate } from "react-router-dom";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { HttpError } from "@/api/errors/HttpError";
import { ErrorCard } from "@/components/cards/ErrorCard";
import { QuizCreatePublicCheckbox } from "@/components/checkbox/QuizCreatePublicCheckbox";
import { QuizCreateTimeLimitInput } from "@/components/input_number/QuizCreateTimeLimitInput";
import { QuizCreateTitleInput } from "@/components/input_str/QuizCreateTitleInput";
import { QuizCreateDescriptionInput } from "@/components/input_str/QuizCreateDescriptionInput";
import { QuizCreateAddQuestionButton } from "@/components/buttons/QuizCreateAddQuestionButton";
import { QuizCreateSubmitButton } from "@/components/buttons/QuizCreateSubmitButton";

export function QuizCreateContainer() {
  //const navigate = useNavigate();

  const [formData, setFormData] = useState<QuizCreateRequestDto>({
    isPublic: false,
    snapshots: [
      {
        title: "",
        description: "",
        TimeLimit: 0,
        questions: [],
      },
    ],
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  const snapshot = formData.snapshots[0];

  // ---------------------------
  // SNAPSHOT UPDATE
  // ---------------------------
  function updateSnapshot(field: string, value: any) {
    const updated = { ...snapshot, [field]: value };

    setFormData((prev) => ({
      ...prev,
      snapshots: [updated],
    }));
  }

  // ---------------------------
  // QUESTION
  // ---------------------------
  function addQuestion() {
    updateSnapshot("questions", [
      ...snapshot.questions,
      {
        title: "",
        text: "",
        mediaUrl: null,
        TimeLimit: 0,
        point: 0,
        answers: [],
      },
    ]);
  }

  function updateQuestion(index: number, field: string, value: any) {
    const updatedQuestions = [...snapshot.questions];
    updatedQuestions[index] = {
      ...updatedQuestions[index],
      [field]: value,
    };

    updateSnapshot("questions", updatedQuestions);
  }

  // ---------------------------
  // ANSWER
  // ---------------------------
  function addAnswer(questionIndex: number) {
    const updatedQuestions = [...snapshot.questions];
    updatedQuestions[questionIndex].answers.push({
      text: "",
      isCorrect: false,
    });

    updateSnapshot("questions", updatedQuestions);
  }

  function updateAnswer(
    questionIndex: number,
    answerIndex: number,
    field: string,
    value: any,
  ) {
    const updatedQuestions = [...snapshot.questions];

    updatedQuestions[questionIndex].answers[answerIndex] = {
      ...updatedQuestions[questionIndex].answers[answerIndex],
      [field]: value,
    };

    updateSnapshot("questions", updatedQuestions);
  }

  // ---------------------------
  // SUBMIT
  // ---------------------------
  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      await createQuiz(formData);
    } catch (e) {
      setError(
        e instanceof HttpError ? e : new HttpError(500, "Unknown error"),
      );
    } finally {
      setIsLoading(false);
    }
  }

  if (isLoading) return <LoadingIndicator />;

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-6">
      <div className="gap-2 grid grid-cols-1 gap-y-6">
        <ErrorCard error={error} />
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-2">
          <QuizCreatePublicCheckbox
            checked={formData.isPublic}
            onChange={(value) =>
              setFormData((prev) => ({
                ...prev,
                isPublic: value,
              }))
            }
          />
          <QuizCreateTimeLimitInput
            value={snapshot.TimeLimit}
            onChange={(value) => updateSnapshot("TimeLimit", value)}
          />
        </div>
        <QuizCreateTitleInput
          value={snapshot.title}
          onChange={(value) => updateSnapshot("title", value)}
        />

        <QuizCreateDescriptionInput
          value={snapshot.description}
          onChange={(value) => updateSnapshot("description", value)}
        />

        {snapshot.questions.map((q, qIndex) => (
          <div key={qIndex} className="border p-4 rounded">
            <input
              placeholder="Question title"
              value={q.title}
              onChange={(e) => updateQuestion(qIndex, "title", e.target.value)}
            />

            <button type="button" onClick={() => addAnswer(qIndex)}>
              + Answer
            </button>

            {q.answers.map((a, aIndex) => (
              <div key={aIndex}>
                <input
                  placeholder="Answer text"
                  value={a.text}
                  onChange={(e) =>
                    updateAnswer(qIndex, aIndex, "text", e.target.value)
                  }
                />
              </div>
            ))}
          </div>
        ))}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
          {/* QUESTIONS */}
          <QuizCreateAddQuestionButton onClick={addQuestion} />
          <QuizCreateSubmitButton onClick={handleSubmit} />
        </div>
      </div>
    </form>
  );
}
