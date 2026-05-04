import type { User } from "@/shared/types/user";

export type NickNameUpdateRequestDto = {
  nickName: string;
};

export type UserStateDto = {
  user: User | null;
  loading: boolean;
};

export type UserResponseDto = {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;
};
