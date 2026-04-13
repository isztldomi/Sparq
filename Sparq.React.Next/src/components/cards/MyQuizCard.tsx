//import { useNavigate } from "react-router-dom";

import { QuizSoftDeleteButton } from "@/components/buttons/QuizSoftDeleteButton";
import { QuizModifyButton } from "@/components/buttons/QuizModifyButton";
import { QuizSessionButton } from "@/components/buttons/QuizSessionButton";

type QuizCardProps = {
  isPublic: boolean;
  lastSnapshot: {
    id: number;
    title: string;
    description: string;
  };
};

export function MyQuizCard({ isPublic, lastSnapshot }: QuizCardProps) {
  //const navigate = useNavigate();
  const deleteHandle = () => {
    // Implement delete logic here
  };
  const modifyQuizHandle = () => {
    // Implement modify quiz logic here
  };
  const sessionStartHandle = () => {
    // Implement session start logic here
  };
  return (
    <div
      key={lastSnapshot.id}
      className="bg-[var(--surface-4)] p-4 rounded-lg shadow-md"
    >
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h2 className="text-xl font-bold mb-2">{lastSnapshot.title}</h2>
          <p>{lastSnapshot.description}</p>
        </div>

        <div className="flex flex-wrap items-center gap-2">
          <p className="text-sm">{isPublic ? "Public" : "Private"}</p>
          <QuizSessionButton onClick={sessionStartHandle} />
          <QuizModifyButton onClick={modifyQuizHandle} />
          <QuizSoftDeleteButton onClick={deleteHandle} />
        </div>
      </div>
    </div>
  );
}
