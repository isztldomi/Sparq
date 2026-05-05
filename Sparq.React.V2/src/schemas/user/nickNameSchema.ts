import { z } from "zod";

export const nameSchema = z
  .string()
  .min(1, `Nick Name is required`)
  .min(3, `Nick Name must be at least 3 characters`)
  .max(10, `Nick Name is too long`);
