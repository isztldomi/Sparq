import { z } from "zod";
import { emailSchema } from "./email.schema";
import { passwordSchema } from "./password.schema";

export const registerSchema = z.object({
  firstName: z
    .string()
    .min(1, "First name is required")
    .min(3, "First name must be at least 3 characters")
    .max(10, "First name is too long"),

  lastName: z
    .string()
    .min(1, "Last name is required")
    .min(3, "Last name must be at least 3 characters")
    .max(10, "Last name is too long"),

  nickName: z
    .string()
    .min(1, "Nick name is required")
    .min(3, "Nick name must be at least 3 characters")
    .max(10, "Nick name is too long"),

  email: emailSchema,

  password: passwordSchema,
});

export type RegisterFormData = z.infer<typeof registerSchema>;
