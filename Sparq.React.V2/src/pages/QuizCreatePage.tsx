import { useState } from "react";
import { GreenButton } from "@/components/buttons/greenButton";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import type { SnapshotCreateFromQuizRequestDto } from "@/features/snapshot/snapshotTypes";
import type { QuizCreateRequestDto } from "@/features/quiz/quizTypes";
import { QuizCreatePublicCheckbox } from "@/components/checkbox/QuizCreatePublicCheckbox";
import { QuizCreateTimeLimitInput } from "@/components/input_number/QuizCreateTimeLimitInput";
import { QuizCreatePinCodeInput } from "@/components/input_str/QuizCreatePinCodeInput";
import { QuizCreateTitleInput } from "@/components/input_str/QuizCreateTitleInput";
import { QuizCreateDescriptionInput } from "@/components/input_str/QuizCreateDescriptionInput";

export function QuizCreatePage() {
  const [serverErrors, setServerErrors] = useState<
    { field: string; message: string }[]
  >([]);

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
  // function addQuestion() {
  //   const newQuestion: QuestionCreateRequestDto = {
  //     title: "",
  //     text: "",
  //     mediaUrl: "",
  //     timeLimit: 10,
  //     point: 0,
  //     answers: [],
  //   };
  //
  //   updateSnapshot("questions", [...(snapshot.questions ?? []), newQuestion]);
  // }

  // function updateQuestion(index: number, field: string, value: any) {
  //   const updatedQuestions = [...snapshot.questions];
  //   updatedQuestions[index] = {
  //     ...updatedQuestions[index],
  //     [field]: value,
  //   };
  //
  //   updateSnapshot("questions", updatedQuestions);
  // }
  //
  // function removeQuestion(index: number) {
  //   const updatedQuestions = snapshot.questions.filter((_, i) => i !== index);
  //   updateSnapshot("questions", updatedQuestions);
  // }

  // ---------------------------
  // ANSWER
  // ---------------------------
  // function addAnswer(questionIndex: number) {
  //   const updatedQuestions = [...snapshot.questions];
  //   const answers = updatedQuestions[questionIndex].answers;
  //
  //   if (answers.length >= 10) return;
  //
  //   answers.push({
  //     text: "",
  //     isCorrect: false,
  //   } as AnswerCreateRequestDto);
  //
  //   updateSnapshot("questions", updatedQuestions);
  // }
  //
  // function updateAnswer(
  //   questionIndex: number,
  //   answerIndex: number,
  //   field: string,
  //   value: any,
  // ) {
  //   const updatedQuestions = [...snapshot.questions];
  //
  //   updatedQuestions[questionIndex].answers[answerIndex] = {
  //     ...updatedQuestions[questionIndex].answers[answerIndex],
  //     [field]: value,
  //   };
  //
  //   updateSnapshot("questions", updatedQuestions);
  // }
  //
  // function setCorrectAnswer(questionIndex: number, answerIndex: number) {
  //   const updatedQuestions = [...snapshot.questions];
  //
  //   updatedQuestions[questionIndex].answers = updatedQuestions[
  //     questionIndex
  //   ].answers.map((answer, i) => ({
  //     ...answer,
  //     isCorrect: i === answerIndex,
  //   }));
  //
  //   updateSnapshot("questions", updatedQuestions);
  // }
  //
  // function removeAnswer(questionIndex: number, answerIndex: number) {
  //   const updatedQuestions = [...snapshot.questions];
  //
  //   updatedQuestions[questionIndex].answers = updatedQuestions[
  //     questionIndex
  //   ].answers.filter((_, i) => i !== answerIndex);
  //
  //   updateSnapshot("questions", updatedQuestions);
  // }

  // ---------------------------
  // SUBMIT
  // ---------------------------
  // async function handleSubmit(e: React.FormEvent) {
  //   e.preventDefault();
  //   setError(null);
  //   setIsLoading(true);
  //
  //   try {
  //     await createQuiz(formData);
  //     navigate("/my-quizzes");
  //   } catch (e) {
  //     setError(
  //       e instanceof HttpError ? e : new HttpError(500, "Unknown error"),
  //     );
  //   } finally {
  //     setIsLoading(false);
  //   }
  // }
  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Quiz Create</h1>

        <GreenButton className="w-30 h-10">Done</GreenButton>
      </div>

      <ErrorsContainer serverErrors={serverErrors} />

      {/* <form onSubmit={handleSubmit} className="flex flex-col gap-6"> */}
      <div className="pt-4">
        <form className="flex flex-col gap-6">
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
          <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
            <div>
              <QuizCreateTitleInput
                value={snapshot.title}
                onChange={(value) => updateSnapshot("title", value)}
              />
            </div>

            <QuizCreateDescriptionInput
              value={snapshot.description}
              onChange={(value) => updateSnapshot("description", value)}
            />
          </div>
          <div>{/* kép feltöltése */}</div>
        </form>
      </div>
    </div>
  );
}
