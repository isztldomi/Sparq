import { z } from "zod";
import { questionSchema } from "../question/questionSchema";

export const snapshotSchema = z.object({
  title: z.string().min(3, "Quiz title is required").max(255),

  description: z.string().min(3, "Quiz description is required").max(510),

  timeLimit: z.coerce
    .number()
    .min(10, "Question Time Limit minimum 10 seconds")
    .max(7200, "Question Time Limit maximum 2 hours"),

  pinCode: z
    .string()
    .trim()
    .length(4, "PIN code must be exactly 4 digits")
    .regex(/^[0-9]+$/, "PIN code must be numeric"),

  questions: z
    .array(questionSchema)
    .min(1, "At least one question is required"),
});
