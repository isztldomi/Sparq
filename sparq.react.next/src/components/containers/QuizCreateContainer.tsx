import { useState } from "react";
import { createQuiz } from "@/api/client/quizzes-client";
import type { QuizCreateRequestDto } from "@/api/models/quizDto/QuizCreateRequestDto";
import type { SnapshotCreateFromQuizRequestDto } from "@/api/models/snapshotDto/SnapshotCreateFromQuizRequestDto";
import { useNavigate } from "react-router-dom";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { HttpError } from "@/api/errors/HttpError";
import { ErrorCard } from "@/components/cards/ErrorCard";
import { QuizCreatePublicCheckbox } from "@/components/checkbox/QuizCreatePublicCheckbox";
import { QuizCreateTimeLimitInput } from "@/components/input_number/QuizCreateTimeLimitInput";
import { QuizCreateTitleInput } from "@/components/input_str/QuizCreateTitleInput";
import { QuizCreateDescriptionInput } from "@/components/input_str/QuizCreateDescriptionInput";
import { QuizCreateAddQuestionButton } from "@/components/buttons/QuizCreateAddQuestionButton";
import { QuizCreateSubmitButton } from "@/components/buttons/QuizCreateSubmitButton";
import { QuizCreateAddAnswerButton } from "@/components/buttons/QuizCreateAddAnswerButton";
import { QuizCreateRemoveQuestionButton } from "@/components/buttons/QuizCreateRemoveQuestionButton";
import { QuizCreateRemoveAndswerButton } from "@/components/buttons/QuizCreateRemoveAndswerButton";
import { QuizCreatePinCodeInput } from "@/components/input_str/QuizCreatePinCodeInput";
import { QuizCreateQuestionTimeLimitInput } from "@/components/input_str/QuizCreateQuestionTimeLimitInput";
import type { QuestionCreateRequestDto } from "@/api/models/questionDto/QuestionCreateRequestDto";
import { QuizCreateQuestionTitleInput } from "@/components/input_str/QuizCreateQuestionTitleInput";
import { QuizCreateQuestionTextArea } from "@/components/areas/QuizCreateQuestionTextArea";
import type { AnswerCreateRequestDto } from "@/api/models/answerDto/AnswerCreateRequestDto";
import { QuizCreateQuestionPointInput } from "@/components/input_number/QuizCreateQuestionPointInput";

export function QuizCreateContainer() {
  const navigate = useNavigate();

  const initialSnapshot: SnapshotCreateFromQuizRequestDto = {
    title: "",
    description: "",
    timeLimit: 10,
    pinCode: "",
    questions: [],
  };

  const [formData, setFormData] = useState<QuizCreateRequestDto>({
    isPublic: false,
    snapshots: [initialSnapshot],
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
    const newQuestion: QuestionCreateRequestDto = {
      title: "",
      text: "",
      mediaUrl: "",
      timeLimit: 10,
      point: 0,
      answers: [],
    };

    updateSnapshot("questions", [...(snapshot.questions ?? []), newQuestion]);
  }

  function updateQuestion(index: number, field: string, value: any) {
    const updatedQuestions = [...snapshot.questions];
    updatedQuestions[index] = {
      ...updatedQuestions[index],
      [field]: value,
    };

    updateSnapshot("questions", updatedQuestions);
  }

  function removeQuestion(index: number) {
    const updatedQuestions = snapshot.questions.filter((_, i) => i !== index);
    updateSnapshot("questions", updatedQuestions);
  }

  // ---------------------------
  // ANSWER
  // ---------------------------
  function addAnswer(questionIndex: number) {
    const updatedQuestions = [...snapshot.questions];
    const answers = updatedQuestions[questionIndex].answers;

    if (answers.length >= 10) return;

    answers.push({
      text: "",
      isCorrect: false,
    } as AnswerCreateRequestDto);

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

  function setCorrectAnswer(questionIndex: number, answerIndex: number) {
    const updatedQuestions = [...snapshot.questions];

    updatedQuestions[questionIndex].answers = updatedQuestions[
      questionIndex
    ].answers.map((answer, i) => ({
      ...answer,
      isCorrect: i === answerIndex,
    }));

    updateSnapshot("questions", updatedQuestions);
  }

  function removeAnswer(questionIndex: number, answerIndex: number) {
    const updatedQuestions = [...snapshot.questions];

    updatedQuestions[questionIndex].answers = updatedQuestions[
      questionIndex
    ].answers.filter((_, i) => i !== answerIndex);

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
      navigate("/my-quizzes");
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
        <div className="grid grid-cols-1 md:grid-cols-3 gap-2">
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
            value={snapshot.timeLimit}
            onChange={(value) => updateSnapshot("timeLimit", value)}
          />
          <QuizCreatePinCodeInput
            value={snapshot.pinCode}
            onChange={(value) => updateSnapshot("pinCode", value)}
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
          <div className="gap-2 bg-[var(--surface-4)] p-5 rounded-lg px-10">
            <div
              key={qIndex}
              className="grid grid-cols-1 md:grid-cols-3 gap-4 py-4"
            >
              <div className="flex items-center gap-2 bg-[var(--surface-5)] p-2 rounded-lg">
                <QuizCreateRemoveQuestionButton
                  onClick={() => removeQuestion(qIndex)}
                />
                <QuizCreateQuestionTitleInput
                  value={q.title}
                  onChange={(value) => updateQuestion(qIndex, "title", value)}
                />
              </div>
              <QuizCreateQuestionTimeLimitInput
                value={q.timeLimit}
                onChange={(value) => updateQuestion(qIndex, "timeLimit", value)}
              />
              <QuizCreateQuestionPointInput
                value={q.point}
                onChange={(value) => updateQuestion(qIndex, "point", value)}
              />
            </div>
            <QuizCreateQuestionTextArea
              value={q.text}
              onChange={(value) => updateQuestion(qIndex, "text", value)}
            />
            <div className="pt-4">
              {q.answers.map((a, aIndex) => (
                <div className="pt-2 px-10" key={aIndex}>
                  <div className="w-full grid grid-cols-1 md:grid-cols-2 gap-2 bg-[var(--surface-5)] p-2 rounded-lg">
                    <div className="flex items-center gap-x-4 pl-1">
                      <QuizCreateRemoveAndswerButton
                        onClick={() => removeAnswer(qIndex, aIndex)}
                      />
                      <input
                        placeholder="Answer text"
                        value={a.text}
                        onChange={(e) =>
                          updateAnswer(qIndex, aIndex, "text", e.target.value)
                        }
                      />
                    </div>
                    <button
                      type="button"
                      onClick={() => setCorrectAnswer(qIndex, aIndex)}
                      className={`px-2 py-1 rounded ${
                        a.isCorrect
                          ? "bg-[var(--success-bg)] text-[var(--success-text)] hover:bg-[var(--success-text)] hover:text-[var(--success-bg)]"
                          : "bg-[var(--error-bg)] text-[var(--error-text)] hover:bg-[var(--error-text)] hover:text-[var(--error-bg)]"
                      }`}
                    >
                      {a.isCorrect ? "Correct" : "Set correct"}
                    </button>
                  </div>
                </div>
              ))}
            </div>
            {q.answers.length < 10 && (
              <QuizCreateAddAnswerButton onClick={() => addAnswer(qIndex)} />
            )}
          </div>
        ))}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
          <QuizCreateAddQuestionButton onClick={addQuestion} />
          <QuizCreateSubmitButton onClick={handleSubmit} />
        </div>
      </div>
    </form>
  );
}
