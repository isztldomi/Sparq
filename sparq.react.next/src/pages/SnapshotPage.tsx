import { useParams } from "react-router-dom";

export function SnapshotPage() {
  const { id } = useParams();
  const snapshotId = Number(id);

  return (
    <div>
      <h1>Snapshot Page</h1>
      <p>Displaying details for snapshot with ID: {snapshotId}</p>
    </div>
  );
}
