import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { useAppDispatch } from "@/app/hooks";
import { login } from "@/features/auth/auth.thunks";
import { flattenErrors } from "@/api/errors/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";

export function LoginPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setErrors([]); // reset

    dispatch(login({ email, password }))
      .unwrap()
      .then(() => navigate("/profile"))
      .catch((err) => {
        if (err?.errors) {
          setErrors(flattenErrors(err.errors));
        }
      });
  };

  return (
    <div className="min-h-screen justify-center p-4">
      <div>
        <h1 className="">Login</h1>
      </div>

      <ErrorsContainer errors={errors} />

      <div className="flex justify-center pt-30">
        <div className="sm:min-w-[200px] md:min-w-[400px] bg-[var(--surface-4)] p-6 rounded-lg shadow-md">
          <form onSubmit={handleSubmit} className="flex flex-col gap-4 ">
            <div>
              <label className="block mb-1">Email</label>
              <input
                type="email"
                className="w-full p-2 rounded border"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            <div>
              <label className="block mb-1">Password</label>
              <input
                type="password"
                className="w-full p-2 rounded border"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <GreenButton type="submit" className="w-full py-2 text-lg">
              Login
            </GreenButton>
          </form>
        </div>
      </div>
    </div>
  );
}
