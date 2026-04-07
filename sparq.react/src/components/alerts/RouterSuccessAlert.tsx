import { useLocation } from "react-router-dom";

export function RouterSuccessAlert() {
  const location = useLocation();

  if (!location.state?.success) {
    return null;
  }

  return (
    <div className="rounded-2xl border border-green-300 bg-green-50 p-4 shadow-sm">
      <h3 className="mb-2 text-lg font-semibold text-green-800">Success</h3>
      <p className="text-green-700">{location.state.success}</p>
    </div>
  );
}
