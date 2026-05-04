type ErrorItem = {
  field: string;
  message: string;
};

interface ErrorsContainerProps {
  serverErrors: ErrorItem[];
}

export function ErrorsContainer({ serverErrors }: ErrorsContainerProps) {
  if (!serverErrors || serverErrors.length === 0) return null;

  return (
    <div className="mb-4 p-3 bg-[var(--error-bg)] text-[var(--error-text)] rounded-lg">
      <ul className="space-y-1">
        {serverErrors.map((err, i) => (
          <li key={i}>
            <strong>{err.field}:</strong> {err.message}
          </li>
        ))}
      </ul>
    </div>
  );
}
