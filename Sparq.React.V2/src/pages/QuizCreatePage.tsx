import { useState } from "react";
import { GreenButton } from "@/components/buttons/greenButton";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";

export function QuizCreatePage() {
  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Quiz Create</h1>

        <GreenButton className="w-30 h-10">Done</GreenButton>
      </div>

      <ErrorsContainer errors={errors} />
    </div>
  );
}
