import { z } from "zod";
import { questionSchema } from "../question/questionSchema";
import { pinCodeSchema } from "./pinCodeSchema";

export const snapshotSchema = z.object({
  title: z.string().min(3, "Quiz title is required").max(255),

  description: z.string().min(3, "Quiz description is required").max(510),

  timeLimit: z.coerce
    .number()
    .min(10, "Question Time Limit minimum 10 seconds")
    .max(7200, "Question Time Limit maximum 2 hours"),

  pinCode: pinCodeSchema,

  questions: z
    .array(questionSchema)
    .min(1, "At least one question is required"),
});
