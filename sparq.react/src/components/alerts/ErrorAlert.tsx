interface Props {
  message: string;
}

export function ErrorAlert({ message }: Props) {
  return (
    <div className="rounded-2xl border border-red-300 bg-red-50 p-4 shadow-sm">
      <h3 className="mb-2 text-lg font-semibold text-red-800">Error</h3>
      <p className="text-red-700">{message}</p>
    </div>
  );
}
