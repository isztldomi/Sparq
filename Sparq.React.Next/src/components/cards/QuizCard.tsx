import { useNavigate } from "react-router-dom";

import { QuizViewButton } from "@/components/buttons/QuizViewButton";

type QuizCardProps = {
  lastSnapshot: {
    id: number;
    title: string;
    description: string;
  };
};

export function QuizCard({ lastSnapshot }: QuizCardProps) {
  const navigate = useNavigate();
  const handleClick = () => {
    navigate(`/demo/snapshot/${lastSnapshot.id}`);
  };
  return (
    <div
      key={lastSnapshot.id}
      className="bg-[var(--surface-4)] p-4 rounded-lg shadow-md"
    >
      <div className="grid grid-cols-1 md:grid-cols-2 items-center gap-4">
        <div>
          <h2 className="text-xl font-bold mb-2">{lastSnapshot.title}</h2>
          <p>{lastSnapshot.description}</p>
        </div>
        <div className="justify-self-end mt-4 md:mt-0">
          <QuizViewButton onClick={handleClick} />
        </div>
      </div>
    </div>
  );
}
