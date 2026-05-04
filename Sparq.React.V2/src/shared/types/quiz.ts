export type Quiz = {
  id: number;
  ownerId: string;
  isPublic: boolean;
  isActive: boolean;
  lastSnapshotId?: number;

  createdAt: Date;
  updatedAt: Date;
};
