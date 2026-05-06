import { z } from "zod";
import { snapshotSchema } from "../snapshot/snapshotSchema";

export const quizSchema = z.object({
  isPublic: z.boolean(),

  snapshots: z
    .array(snapshotSchema)
    .length(1, "Exactly one snapshot is required"),
});
