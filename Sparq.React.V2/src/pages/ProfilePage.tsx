import { useNavigate } from "react-router-dom";
import { useAppSelector } from "@/app/hooks";
import { selectUser } from "@/features/auth/auth.selectors";
import { GreenButton } from "@/components/buttons/greenButton";
import { RedButton } from "@/components/buttons/redButton";

export function ProfilePage() {
  const user = useAppSelector(selectUser);
  const navigate = useNavigate();

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Profile</h1>
        {user ? (
          <RedButton className="w-30 h-10" onClick={() => navigate("/logout")}>
            Logout
          </RedButton>
        ) : (
          <></>
        )}
      </div>

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
        <div>
          <div>First Name: {user.firstName}</div>
          <div>Last Name: {user.lastName}</div>
          <div>Nick Name: {user.nickName}</div>
          <div>Email: {user.email}</div>
        </div>
      )}
    </div>
  );
}
