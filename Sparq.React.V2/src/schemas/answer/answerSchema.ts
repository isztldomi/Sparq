import { z } from "zod";

export const answerSchema = z.object({
  id: z.string().optional(),

  text: z
    .string()
    .min(1, "Answer text is required")
    .max(255, "Answer is too long"),

  isCorrect: z.boolean(),
});

export const answersSchema = z
  .array(answerSchema)
  .min(1, "At least one answer is required")
  .max(10, "Maximum 10 answers allowed");
