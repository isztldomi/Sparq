import { z } from "zod";

export const pinCodeSchema = z
  .string()
  .trim()
  .length(4, "PIN code must be exactly 4 digits")
  .regex(/^[0-9]+$/, "PIN code must be numeric");
