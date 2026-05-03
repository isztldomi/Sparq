import { GreenButton } from "../buttons/greenButton";

interface ProfilDetailsContainerProps {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;

  value: string;
  onNickNameChange: (value: string) => void;
  onSave: () => void;
  isSaving?: boolean;
}

interface DetailsContainerProps {
  label: string;
  value: string;
}

function DetailsContainer({ label, value }: DetailsContainerProps) {
  return (
    <div className="bg-[var(--surface-4)] p-5 rounded-lg w-full">
      <span>{label}</span>
      <span>: </span>
      <span>{value}</span>
    </div>
  );
}

export function ProfilDetailsContainer({
  firstName,
  lastName,
  nickName,
  email,
  value,
  onNickNameChange,
  onSave,
  isSaving,
}: ProfilDetailsContainerProps) {
  const isDirty = value !== nickName;

  return (
    <div className="flex flex-col gap-4">
      <div className="bg-[var(--surface-4)] rounded-lg p-5">
        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-2 w-full">
            <span>Nick Name:</span>
            <input
              value={value}
              onChange={(e) => onNickNameChange(e.target.value)}
              className="bg-transparent border px-2 py-1 w-full"
            />
          </div>

          <GreenButton
            className="w-20 h-10"
            disabled={!isDirty || isSaving}
            onClick={onSave}
          >
            Save
          </GreenButton>
        </div>
      </div>

      <div className="flex flex-col w-full gap-5">
        <DetailsContainer label="First Name" value={firstName} />
        <DetailsContainer label="Last Name" value={lastName} />
        <DetailsContainer label="Email" value={email} />
      </div>
    </div>
  );
}
