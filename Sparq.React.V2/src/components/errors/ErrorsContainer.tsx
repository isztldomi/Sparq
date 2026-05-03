type ErrorItem = {
  field: string;
  message: string;
};

interface ErrorsContainerProps {
  errors: ErrorItem[];
}

export function ErrorsContainer({ errors }: ErrorsContainerProps) {
  if (!errors || errors.length === 0) return null;

  return (
    <div className="mb-4 p-3 bg-[var(--error-bg)] text-[var(--error-text)] rounded-lg">
      <ul className="space-y-1">
        {errors.map((err, i) => (
          <li key={i}>
            <strong>{err.field}:</strong> {err.message}
          </li>
        ))}
      </ul>
    </div>
  );
}
