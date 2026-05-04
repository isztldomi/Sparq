import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";

export function MyQuizzesPage() {
  const navigate = useNavigate();

  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">My Quizzes</h1>

        <GreenButton
          className="w-30 h-10"
          onClick={() => navigate("/quiz/create")}
        >
          Quiz Create
        </GreenButton>
      </div>

      <ErrorsContainer serverErrors={errors} />
    </div>
  );
}
