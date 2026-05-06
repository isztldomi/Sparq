import { type ZodIssue } from "zod";

export function buildErrorMap(issues: ZodIssue[]): Record<string, string> {
  const errorMap: Record<string, string> = {};

  for (const issue of issues) {
    const path = issue.path.join(".");

    if (!errorMap[path]) {
      errorMap[path] = issue.message;
    }
  }

  return errorMap;
}
