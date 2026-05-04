import type { Snapshot } from "./snapshot";

export type Quiz = {
  id: number;
  ownerId: string;
  isPublic: boolean;
  isActive: boolean;
  lastSnapshotId: number;
  snapshots: Snapshot[];

  createdAt: Date;
  updatedAt: Date;
};
