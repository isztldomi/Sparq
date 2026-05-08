import type { Snapshot } from "./snapshot";

export type Quiz = {
  id: string;
  ownerId: string;
  isPublic: boolean;
  isActive: boolean;
  lastSnapshotId: string;
  snapshots: Snapshot[];

  createdAt: Date;
  updatedAt: Date;
};
