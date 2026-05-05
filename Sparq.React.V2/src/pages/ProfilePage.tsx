import { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import {
  useGetProfileQuery,
  useUpdateNickNameMutation,
} from "@/features/user/userApi";
import { GreenButton } from "@/components/buttons/greenButton";
import { RedButton } from "@/components/buttons/redButton";
import { ProfilDetailsContainer } from "@/components/containers/ProfileDetailsContainer";
import { flattenErrors } from "@/api/core/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import type { ProblemDetails } from "@/api/models/ProblemDetails";
import { nameSchema } from "@/schemas/user/nickNameSchema";

export function ProfilePage() {
  const { data: user, isLoading } = useGetProfileQuery();
  const [updateNickName] = useUpdateNickNameMutation();

  const navigate = useNavigate();

  const [serverErrors, setServerErrors] = useState<
    { field: string; message: string }[]
  >([]);

  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});

  const [nickName, setNickName] = useState("");
  const initRef = useRef(false);

  useEffect(() => {
    if (!initRef.current && user?.nickName) {
      setNickName(user.nickName);
      initRef.current = true;
    }
  }, [user]);

  const handleSave = async () => {
    setServerErrors([]);
    setFieldErrors({});

    const result = nameSchema.safeParse(nickName);

    if (!result.success) {
      setFieldErrors({
        nickName: result.error.issues[0].message,
      });
      return;
    }

    try {
      await updateNickName({ nickName }).unwrap();
    } catch (err: unknown) {
      const error = err as { data?: ProblemDetails };
      setServerErrors(flattenErrors(error.data?.errors));
    }
  };

  if (isLoading) {
    return <div className="p-4">Loading...</div>;
  }

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Profile</h1>

        {user && (
          <RedButton className="w-30 h-10" onClick={() => navigate("/logout")}>
            Logout
          </RedButton>
        )}
      </div>

      <ErrorsContainer serverErrors={serverErrors} />

      {!user ? (
        <div className="grid grid-cols-1 md:grid-cols-2 pt-30 gap-y-10">
          <div className="flex justify-center">
            <GreenButton
              className="w-50 h-20"
              onClick={() => navigate("/login")}
            >
              <span className="text-2xl">Login</span>
            </GreenButton>
          </div>

          <div className="flex justify-center">
            <GreenButton
              className="w-50 h-20"
              onClick={() => navigate("/register")}
            >
              <span className="text-2xl">Registration</span>
            </GreenButton>
          </div>
        </div>
      ) : (
        <>
          <ProfilDetailsContainer
            firstName={user.firstName}
            lastName={user.lastName}
            nickName={user.nickName}
            email={user.email}
            value={nickName}
            onNickNameChange={setNickName}
            onSave={handleSave}
            nickNameError={fieldErrors.nickName}
          />
        </>
      )}
    </div>
  );
}
