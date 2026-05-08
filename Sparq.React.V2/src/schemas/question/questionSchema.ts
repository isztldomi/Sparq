import { z } from "zod";
import { answersSchema } from "../answer/answerSchema";

export const questionSchema = z
  .object({
    id: z.string().optional(),

    title: z
      .string()
      .min(3, "Question Title must be at least 3 characters")
      .max(255, "Question Title too long"),

    text: z
      .string()
      .min(3, "Question text must be at least 3 characters")
      .max(2000, "Question text too long"),

    mediaId: z.string().nullable().optional(),

    timeLimit: z
      .number()
      .min(10, "Question Time Limit minimum 10 seconds")
      .max(7200, "Question Time Limit maximum 2 hours"),

    point: z.number().min(0, "Min point is 0").max(10, "Max point is 10"),

    answers: answersSchema,
  })
  .superRefine((q, ctx) => {
    const correctCount = q.answers.filter((a) => a.isCorrect).length;

    if (correctCount !== 1) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Exactly one correct answer is required",

        path: ["answers"],
      });
    }
  });
