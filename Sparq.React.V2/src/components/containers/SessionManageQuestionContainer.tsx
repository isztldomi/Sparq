import { useEffect, useMemo, useState } from "react";

import { useNextQuestionSessionMutation } from "@/features/session/sessionApi";

import { useGetCurrentQuestionWithResultQuery } from "@/features/question/questionApi";

import { useGetMediaBlobQuery } from "@/features/media/mediaApi";

import { LoadingIndicator } from "../loadings/LoadingIndicator";
import { GreenRedCheckbox } from "../checkbox/greenRedCheckbox";
import { GreenButton } from "../buttons/greenButton";

type Props = {
  sessionId: string;
};

export function SessionManageQuestionContainer({ sessionId }: Props) {
  const [showCorrect, setShowCorrect] = useState(false);

  const [now, setNow] = useState(Date.now());

  const [nextQuestion, { isLoading: isNextLoading }] =
    useNextQuestionSessionMutation();

  // ----------------------------
  // QUERY
  // ----------------------------
  const { data, isLoading, isError } = useGetCurrentQuestionWithResultQuery({
    sessionId,
  });

  const question = data?.question;

  // ----------------------------
  // IMAGE
  // ----------------------------
  const mediaId = question?.mediaId;

  const { data: imageBlob } = useGetMediaBlobQuery(mediaId!, {
    skip: !mediaId,
  });

  const imageUrl = useMemo(() => {
    if (!imageBlob) return undefined;

    return URL.createObjectURL(imageBlob);
  }, [imageBlob]);

  useEffect(() => {
    return () => {
      if (imageUrl) {
        URL.revokeObjectURL(imageUrl);
      }
    };
  }, [imageUrl]);

  // ----------------------------
  // LIVE TICK
  // ----------------------------
  useEffect(() => {
    const interval = setInterval(() => {
      setNow(Date.now());
    }, 200);

    return () => clearInterval(interval);
  }, []);

  // ----------------------------
  // TIMER
  // ----------------------------
  const timeLeft = data?.endsAt
    ? Math.max(0, new Date(data.endsAt).getTime() - now)
    : 0;

  const totalTime =
    data?.startsAt && data?.endsAt
      ? new Date(data.endsAt).getTime() - new Date(data.startsAt).getTime()
      : 0;

  const progress = totalTime ? (timeLeft / totalTime) * 100 : 0;

  // ----------------------------
  // NEXT QUESTION
  // ----------------------------
  const handleNext = async () => {
    await nextQuestion(sessionId);
  };

  // ----------------------------
  // LOADING / ERROR
  // ----------------------------
  if (isLoading) {
    return <LoadingIndicator />;
  }

  if (isError) {
    return (
      <div className="flex justify-center bg-[var(--surface-4)] rounded-lg p-4">
        <GreenButton
          onClick={handleNext}
          className="w-40 h-10 flex flex-col justify-center gap-1"
        >
          <p>Start first question</p>
        </GreenButton>
      </div>
    );
  }

  // ----------------------------
  // NO ACTIVE QUESTION
  // ----------------------------
  if (!question) {
    return (
      <div className="p-4 bg-[var(--surface-4)] rounded-lg flex flex-col gap-3">
        <span>No active question</span>

        <GreenButton onClick={handleNext} className="w-40 h-10">
          Start first question
        </GreenButton>
      </div>
    );
  }

  // ----------------------------
  // RENDER
  // ----------------------------
  return (
    <div className="flex flex-col gap-4 bg-[var(--surface-4)] p-4 rounded-lg">
      {/* HEADER */}
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold">Question #{data?.order + 1}</h2>

        <GreenRedCheckbox
          value={!showCorrect}
          onChange={(value) => setShowCorrect(!value)}
          falseLabel="Hide correct"
          trueLabel="Show correct"
          className="w-30 h-10"
        />
      </div>

      {/* TITLE */}
      <div className="bg-[var(--surface-5)] p-4 rounded">
        <h3 className="text-md font-medium">{question.title}</h3>
      </div>

      {/* TEXT */}
      <div className="bg-[var(--surface-5)] p-4 rounded">
        <span>{question.text}</span>
      </div>

      {/* MEDIA */}
      {imageUrl && (
        <div className="bg-[var(--surface-5)] p-4 rounded">
          <img
            src={imageUrl}
            alt={question.title}
            className="w-full max-h-[400px] object-contain rounded-lg"
          />
        </div>
      )}

      {/* TIMER */}
      <div className="bg-[var(--surface-5)] p-4 rounded flex flex-col gap-2">
        <div className="flex justify-between text-sm">
          <span>Time left</span>

          <span>{Math.ceil(timeLeft / 1000)}s</span>
        </div>

        <div className="w-full h-2 bg-gray-700 rounded overflow-hidden">
          <div
            className="h-2 bg-green-500 transition-all"
            style={{
              width: `${progress}%`,
            }}
          />
        </div>
      </div>

      {/* ANSWERS */}
      <div className="flex flex-col gap-2">
        <h4 className="font-medium">Answers</h4>

        {question.answers?.map((a: any) => {
          const isCorrect = a.isCorrect;

          return (
            <div
              key={a.id}
              className={`p-3 rounded border transition ${
                showCorrect && isCorrect
                  ? "bg-green-500 text-white border-green-700"
                  : "bg-[var(--surface-5)]"
              }`}
            >
              {a.text}
            </div>
          );
        })}
      </div>

      {/* ACTIONS */}
      <div className="flex justify-end pt-2">
        <GreenButton
          onClick={handleNext}
          className="w-40 h-10"
          disabled={isNextLoading}
        >
          Next question
        </GreenButton>
      </div>
    </div>
  );
}
