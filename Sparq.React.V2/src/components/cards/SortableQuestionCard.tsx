import type { QuestionUI } from "@/features/question/questionTypes";
import { RedButton } from "../buttons/redButton";
import { resizeImage } from "@/utils/image/resizeImage";

type SortableQuestionCardProps = {
  question: QuestionUI;
  index: number;
  total: number;
  onDelete: () => void;
  onUpdate: (field: keyof QuestionUI, value: any) => void;
};

export function SortableQuestionCard({
  question,
  index,
  total,
  onDelete,
  onUpdate,
}: SortableQuestionCardProps) {
  return (
    <div className="bg-[var(--surface-5)] rounded-lg p-4 flex flex-col gap-3">
      {/* TOP ROW */}
      <div className="flex gap-3">
        <RedButton onClick={onDelete} className="w-30 h-10">
          Remove
        </RedButton>

        <textarea
          placeholder="Question text"
          value={question.text}
          onChange={(e) => onUpdate("text", e.target.value)}
          onPointerDown={(e) => e.stopPropagation()}
          className="
            w-full 
            min-h-[80px]
            bg-[var(--surface-4)] 
            text-[var(--text-h)]
            border border-transparent
            focus:border-[var(--surface-3)]
            rounded-lg 
            p-2 
            outline-none 
            resize-none
          "
        />
      </div>

      {/* 2nd ROW - MEDIA */}
      <div className="flex gap-2">
        <input
          type="file"
          accept="image/*"
          onChange={async (e) => {
            const file = e.target.files?.[0];
            if (!file) {
              onUpdate("mediaFile", null);
              onUpdate("mediaPreviewUrl", null);
              return;
            }

            const resized = await resizeImage(file);

            const previewUrl = URL.createObjectURL(resized);

            onUpdate("mediaFile", resized);
            onUpdate("mediaPreviewUrl", previewUrl);
          }}
          className="text-sm text-[var(--text-muted)] bg-[var(--surface-6)]"
        />
      </div>

      {/* 3nd ROW - MEDIA print */}
      <div className="flex gap-2">
        {/* PREVIEW */}
        {question.mediaPreviewUrl && (
          <div className="w-full max-w-[300px]">
            <img
              src={question.mediaPreviewUrl}
              className="w-full h-[150px] object-cover"
            />
          </div>
        )}
      </div>

      {/* META */}
      <div className="text-xs text-[var(--text-muted)]">ID: {question.id}</div>
    </div>
  );
}
