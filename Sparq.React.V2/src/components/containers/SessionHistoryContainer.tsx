import type { SessionStatusResponseDto } from "@/features/session/sessionTypes";

interface Prop {
  statusData: SessionStatusResponseDto;
}

export function SessionHistoryContainer({ statusData }: Prop) {
  return (
    <div>
      <div>asd</div>
      <div>{statusData.status}</div>
    </div>
  );
}
