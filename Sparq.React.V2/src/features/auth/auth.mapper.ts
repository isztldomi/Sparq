import type { User } from "@/shared/types/user";
import type { UserResponseDto } from "@/features/auth/auth.types";

export const mapUser = (dto: UserResponseDto): User => {
  return {
    firstName: dto.firstName,
    lastName: dto.lastName,
    nickName: dto.nickName,
    email: dto.email,
  };
};
