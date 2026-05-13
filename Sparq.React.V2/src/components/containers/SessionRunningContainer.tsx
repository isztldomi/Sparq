import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  useGetCurrentQuestionWithoutResultQuery,
  useGetCurrentQuestionWithResultQuery,
} from "@/features/question/questionApi";

import { useGetMediaBlobSessionQuery } from "@/features/media/mediaApi";

type Props = {
  sessionId: string;
  extUserId?: string;
};

export function SessionRunningContainer({ sessionId, extUserId }: Props) {
  const navigate = useNavigate();

  const [showResult, setShowResult] = useState(false);
  const [now, setNow] = useState(Date.now());

  // ----------------------------
  // QUERIES
  // ----------------------------
  const withoutResultQuery = useGetCurrentQuestionWithoutResultQuery(
    { sessionId, extUserId },
    { skip: showResult },
  );

  const withResultQuery = useGetCurrentQuestionWithResultQuery(
    { sessionId, extUserId },
    { skip: !showResult },
  );

  const activeQuery = showResult ? withResultQuery : withoutResultQuery;

  const data = activeQuery.data;

  const isLoading = withoutResultQuery.isLoading || withResultQuery.isLoading;

  const isError = withoutResultQuery.isError || withResultQuery.isError;

  const question = data?.question;

  // ----------------------------
  // IMAGE
  // ----------------------------
  const mediaId = question?.mediaId;

  const { data: imageBlob } = useGetMediaBlobSessionQuery(
    {
      sessionId,
      mediaId: mediaId!,
      extUserId,
    },
    {
      skip: !mediaId,
    },
  );

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
  // CLOCK (UI refresh)
  // ----------------------------
  useEffect(() => {
    const interval = setInterval(() => {
      setNow(Date.now());
    }, 200);

    return () => clearInterval(interval);
  }, []);

  // ----------------------------
  // TIMER → SWITCH TO RESULT API
  // ----------------------------
  useEffect(() => {
    if (!data?.endsAt) return;

    const remaining = new Date(data.endsAt).getTime() - Date.now();

    if (remaining <= 0) {
      setShowResult(true);
      return;
    }

    const timeout = setTimeout(() => {
      setShowResult(true);
    }, remaining);

    return () => clearTimeout(timeout);
  }, [data?.endsAt]);

  // ----------------------------
  // LOADING / ERROR
  // ----------------------------
  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError || !data) {
    return (
      <div>
        Error loading session question
        <button onClick={() => navigate(-1)}>Go back</button>
      </div>
    );
  }

  // ----------------------------
  // TIMER CALCS
  // ----------------------------
  const timeLeft = Math.max(0, new Date(data.endsAt).getTime() - now);

  const totalTime =
    new Date(data.endsAt).getTime() - new Date(data.startedAt).getTime();

  const progress = totalTime ? (timeLeft / totalTime) * 100 : 0;

  // ----------------------------
  // RENDER
  // ----------------------------
  return (
    <div className="min-h-screen justify-center p-4">
      <h1 className="text-xl">Session Running</h1>

      <div className="pt-4 bg-[var(--surface-4)] p-5 rounded-lg flex flex-col gap-4">
        {/* QUESTION ORDER */}
        <h2 className="text-lg font-semibold">Question #{data?.order + 1}</h2>

        {/* TITLE */}
        <div className="bg-[var(--surface-5)] p-5 rounded-lg">
          <h2>{question?.title}</h2>
        </div>

        {/* TEXT */}
        <div className="bg-[var(--surface-5)] p-5 rounded-lg">
          <span>{question?.text}</span>
        </div>

        {/* IMAGE */}
        {imageUrl && (
          <div className="bg-[var(--surface-5)] p-5 rounded-lg">
            <img
              src={imageUrl}
              alt={question?.title}
              className="w-full max-h-[400px] object-contain rounded-lg"
            />
          </div>
        )}

        {/* TIMER */}
        <div className="flex flex-col gap-2 bg-[var(--surface-5)] p-4 rounded-lg">
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
        <div className="flex flex-col gap-2 bg-[var(--surface-5)] p-5 rounded-lg">
          <h3>Answers</h3>

          {question?.answers?.length ? (
            question.answers.map((a: any) => {
              const isCorrect = showResult && a.isCorrect;

              return (
                <button
                  key={a.id}
                  onClick={() => {}}
                  disabled={showResult}
                  className={`p-3 rounded transition-all ${
                    isCorrect
                      ? "bg-[var(--success-bg)] text-[var(--success-text)]"
                      : "bg-[var(--surface-6)] hover:opacity-80"
                  }`}
                >
                  {a.text}
                </button>
              );
            })
          ) : (
            <span>No answers yet</span>
          )}
        </div>

        {/* RESULT STATE */}
        {showResult && (
          <div className="bg-green-500 p-3 rounded">Results unlocked 🎉</div>
        )}
      </div>
    </div>
  );
}
