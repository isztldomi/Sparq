import type { QuestionUI } from "@/features/question/questionTypes";
import { RedButton } from "../buttons/redButton";
import { resizeImage } from "@/utils/image/resizeImage";
import { AnswersDndContainer } from "../containers/AnswersDndContainer";
import { GreenButton } from "../buttons/greenButton";
import type { AnswerUI } from "@/features/answer/answerTypes";

type SortableQuestionCardProps = {
  question: QuestionUI;
  indexQuestion: number;
  totalQuestion: number;
  onDeleteQuestion: () => void;
  onUpdateQuestion: (field: keyof QuestionUI, value: any) => void;
  onAddAnswer: () => void;
  onUpdateAnswer: (answerId: string, field: keyof AnswerUI, value: any) => void;
  onDeleteAnswer: (answerId: string) => void;
  getClientError: (path: string) => string | undefined;
};

export function SortableQuestionCard({
  question,
  indexQuestion,
  totalQuestion,
  onDeleteQuestion,
  onUpdateQuestion,
  onAddAnswer,
  onUpdateAnswer,
  onDeleteAnswer,
  getClientError,
}: SortableQuestionCardProps) {
  return (
    <div className="rounded-lg p-4 flex flex-col gap-3">
      {/* TOP ROW */}
      <div className="flex gap-3">
        <RedButton onClick={onDeleteQuestion} className="w-30 h-10">
          Remove
        </RedButton>

        <textarea
          placeholder="Question text"
          value={question.text}
          onChange={(e) => onUpdateQuestion("text", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          className={`w-full min-h-[80px] bg-[var(--surface-5)] text-[var(--text-h)] rounded-lg p-2 outline-none resize-none border
            ${
              getClientError(`snapshots.0.questions.${indexQuestion}.text`)
                ? "border-[var(--error-text)]"
                : "border-transparent"
            }`}
        />
      </div>

      {/* 2nd ROW - MEDIA */}
      <div className="flex flex-wrap gap-2 items-center">
        <label className="cursor-pointer bg-[var(--surface-6)] px-4 py-2 rounded-lg text-sm text-[var(--text-muted)]">
          Upload image
          <input
            type="file"
            accept="image/*"
            onChange={async (e) => {
              const file = e.target.files?.[0];
              if (!file) {
                onUpdateQuestion("mediaId", null);
                onUpdateQuestion("mediaFile", null);
                onUpdateQuestion("mediaPreviewUrl", null);
                return;
              }

              const resized = await resizeImage(file);
              const previewUrl = URL.createObjectURL(resized);

              onUpdateQuestion("mediaFile", resized);
              onUpdateQuestion("mediaPreviewUrl", previewUrl);
            }}
            className="hidden"
          />
        </label>

        {question.mediaFile && (
          <span className="text-sm text-[var(--text-h)]">
            {question.mediaFile.name}
          </span>
        )}
        <div className="flex gap-2">
          {/* PREVIEW */}
          {question.mediaPreviewUrl && (
            <div className="w-full max-w-[300px]">
              <img
                src={question.mediaPreviewUrl ?? undefined}
                onLoad={() => console.log("IMAGE LOADED")}
                onError={() => console.log("IMAGE ERROR")}
                className="w-full h-[150px] object-cover"
              />
            </div>
          )}
        </div>
      </div>

      {/* 4nd ROW - Answres */}
      <AnswersDndContainer
        answers={question.answers}
        indexQuestion={indexQuestion}
        onReorderAnswer={(answers) => onUpdateQuestion("answers", answers)}
        onUpdateAnswer={(answerId, field, value) =>
          onUpdateAnswer(answerId, field, value)
        }
        onDeleteAnswer={(answerId) => onDeleteAnswer(answerId)}
        getClientError={getClientError}
      />

      <div className="flex justify-center">
        <GreenButton onClick={onAddAnswer} className="w-40 h-10">
          + Answer
        </GreenButton>
      </div>

      {/* META <div className="text-xs text-[var(--text-muted)]">ID: {question.id}</div> */}
    </div>
  );
}
