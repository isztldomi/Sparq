import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getSnapshotById } from "@/api/client/snapshots-client";
import type { SnapshotResponseDto } from "@/api/models/snapshotDto/SnapshotResponseDto";
import { LoadingIndicator } from "@/components/LoadingIndicator";

export function TempAnswerPage() {
  const { snapshotId } = useParams();

  const [snapshot, setSnapshot] = useState<SnapshotResponseDto | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [currentIndex, setCurrentIndex] = useState(0);

  // lezárt válaszok
  const [answered, setAnswered] = useState<Record<number, number>>({});

  const [isFinished, setIsFinished] = useState(false);

  useEffect(() => {
    async function load() {
      if (!snapshotId) return;

      setIsLoading(true);
      setError(null);

      try {
        const data = await getSnapshotById(Number(snapshotId));
        setSnapshot(data);
      } catch (e) {
        setError(e instanceof Error ? e.message : "Unknown error");
      } finally {
        setIsLoading(false);
      }
    }

    load();
  }, [snapshotId]);

  if (isLoading) return <LoadingIndicator />;
  if (error) return <div className="text-red-500">{error}</div>;
  if (!snapshot) return null;

  const questions = snapshot.questions;
  const currentQuestion = questions[currentIndex];

  const selectedId = answered[currentQuestion.id];
  const correctAnswer = currentQuestion.answers.find((a) => a.isCorrect);

  function selectAnswer(answerId: number) {
    if (answered[currentQuestion.id]) return;

    setAnswered((prev) => ({
      ...prev,
      [currentQuestion.id]: answerId,
    }));
  }

  function next() {
    if (currentIndex < questions.length - 1) {
      setCurrentIndex((i) => i + 1);
    } else {
      setIsFinished(true);
    }
  }

  function calculateScore() {
    let score = 0;

    for (const q of questions) {
      const selected = answered[q.id];
      const correct = q.answers.find((a) => a.isCorrect);

      if (correct && selected === correct.id) {
        score++;
      }
    }

    return score;
  }

  // 🏁 RESULT VIEW
  if (isFinished) {
    const score = calculateScore();

    return (
      <div className="mb-4 py-4">
        <h2 className="h2-style">Result</h2>

        <div className="flex gap-3 flex-col bg-[var(--surface-4)] rounded-lg p-4 mb-4 shadow">
          <p className="mb-4 ">
            {score} / {questions.length} correct
          </p>
          {questions.map((q) => {
            const selected = answered[q.id];
            const correct = q.answers.find((a) => a.isCorrect);

            return (
              <div className="bg-[var(--surface-5)] rounded-lg p-4 mb-4 shadow">
                <div key={q.id} className="mb-4">
                  <h3 className="font-bold">{q.title}</h3>
                  <p>{q.text}</p>

                  <ul className="mt-2">
                    {q.answers.map((a) => {
                      const isSelected = selected === a.id;
                      const isCorrect = a.isCorrect;

                      let className = "";

                      if (isCorrect) {
                        className = "text-green-600";
                      } else if (isSelected && !isCorrect) {
                        className = "text-red-600";
                      }

                      return (
                        <li key={a.id} className={className}>
                          {a.text}
                        </li>
                      );
                    })}
                  </ul>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    );
  }

  // 🧩 QUESTION VIEW
  return (
    <div className="mb-4 py-4">
      <h2 className="h2-style">
        {currentIndex + 1} / {questions.length}
      </h2>

      <div className="w-full bg-[var(--surface-4)] rounded-lg mb-4 p-4 shadow">
        <div className="mb-4">
          <h3 className="mb-4 font-bold">{currentQuestion.title}</h3>
          <p>{currentQuestion.text}</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
          {currentQuestion.answers.map((a) => {
            const isSelected = selectedId === a.id;
            const isCorrect = correctAnswer?.id === a.id;

            let className = "p-2 rounded-lg bg-[var(--surface-5)]";

            if (selectedId) {
              if (isCorrect) {
                className =
                  "p-2 rounded-lg bg-[var(--success-bg)] text-[var(--success-text)]";
              } else if (isSelected && !isCorrect) {
                className =
                  "p-2 rounded-lg bg-[var(--error-bg)] text-[var(--error-text)]";
              }
            }

            return (
              <button
                key={a.id}
                onClick={() => selectAnswer(a.id)}
                disabled={!!selectedId}
                className={className}
              >
                {a.text}
              </button>
            );
          })}
        </div>
      </div>

      <button
        onClick={next}
        disabled={!selectedId}
        className="mt-4 px-4 py-2 rounded-lg bg-[var(--surface-4)] disabled:bg-[var(--surface-5)] disabled:text-[var(--surface-4)]"
      >
        {currentIndex === questions.length - 1 ? "Befejezés" : "Következő"}
      </button>
    </div>
  );
}
