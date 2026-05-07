import { useGetQuizByIdApiQuery } from "@/features/quiz/quizApi";
import { useParams } from "react-router-dom";

export function QuizModifyPage() {
  const { id } = useParams();

  const { data, isLoading } = useGetQuizByIdApiQuery(Number(id));

  console.log("DATA: " + data);
  return (
    <div>
      <div>asd</div>
    </div>
  );
}
