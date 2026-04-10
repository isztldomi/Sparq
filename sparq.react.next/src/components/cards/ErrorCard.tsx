import { ServerSideValidationError } from "@/api/errors/ServerSideValidationError";

type ErrorCardProps = {
  error: Error | null;
};

export function ErrorCard({ error }: ErrorCardProps) {
  if (!error) return null;

  return (
    <div className="bg-[var(--error-bg)] text-[var(--error-text)] p-4 rounded-lg">
      <div>{error.message}</div>

      {error instanceof ServerSideValidationError && (
        <ul>
          {Object.entries(error.validationErrors).map(([field, message]) => (
            <li key={field}>
              {field}: {message}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
