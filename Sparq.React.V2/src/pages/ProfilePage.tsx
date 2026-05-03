//import { useState, useEffect } from "react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppSelector, useAppDispatch } from "@/app/hooks";
import { selectUser } from "@/features/user/user.selectors";
import { GreenButton } from "@/components/buttons/greenButton";
import { RedButton } from "@/components/buttons/redButton";
import { ProfilDetailsContainer } from "@/components/containers/ProfileDetailsContainer";
import { nickNameUpdate } from "@/features/user/user.thunks";
import { flattenErrors } from "@/api/errors/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import type { ProblemDetails } from "@/api/models/ProblemDetails";

export function ProfilePage() {
  const user = useAppSelector(selectUser);
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [nickName, setNickName] = useState(() => user?.nickName ?? "");

  //useEffect(() => {
  //  setNickName(user?.nickName ?? "");
  //}, [user]);

  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  const handleSave = async () => {
    setErrors([]);

    try {
      await dispatch(nickNameUpdate({ nickName })).unwrap();
    } catch (err: unknown) {
      const error = err as ProblemDetails;

      setErrors(flattenErrors(error.errors));
    }
  };

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

      <ErrorsContainer errors={errors} />

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
        <ProfilDetailsContainer
          firstName={user.firstName}
          lastName={user.lastName}
          nickName={user.nickName}
          email={user.email}
          value={nickName}
          onNickNameChange={setNickName}
          onSave={handleSave}
        />
      )}
    </div>
  );
}
