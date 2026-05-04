import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { useAppDispatch } from "@/app/hooks";
// import { login } from "@/features/auth/auth.thunks";
import { flattenErrors } from "@/api/core/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import type { ProblemDetails } from "@/api/models/ProblemDetails";
import { useLoginMutation } from "@/features/auth/authApi";
import { setAuth } from "@/features/auth/authSlice";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginFormData } from "@/schemas/auth/loginSchema";

export function LoginPage() {
  //const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  const {
    register,
    handleSubmit,
    formState: { errors: formErrors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  const [login, { isLoading }] = useLoginMutation();
  const dispatch = useAppDispatch();

  const onSubmit = async (data: LoginFormData) => {
    setErrors([]);

    try {
      const res = await login(data).unwrap();

      const payload = {
        token: res.authToken,
        refreshToken: res.refreshToken,
      };

      localStorage.setItem("auth", JSON.stringify(payload));

      dispatch(setAuth(payload));

      navigate("/profile");
    } catch (err: unknown) {
      const error = err as { data?: ProblemDetails };
      setErrors(flattenErrors(error.data?.errors));
    }
  };

  //  const onSubmit = async (data: LoginFormData) => {
  //    setErrors([]);
  //
  //    try {
  //      await dispatch(login(data)).unwrap();
  //      navigate("/profile");
  //    } catch (err: unknown) {
  //      const error = err as ProblemDetails;
  //
  //      setErrors(flattenErrors(error.errors));
  //    }
  //  };

  return (
    <div className="min-h-screen justify-center p-4">
      <h1>Login</h1>

      <ErrorsContainer serverErrors={errors} />

      <div className="flex justify-center pt-30">
        <div className="sm:min-w-[200px] md:min-w-[400px] bg-[var(--surface-4)] p-6 rounded-lg shadow-md">
          <form
            onSubmit={handleSubmit(onSubmit)}
            className="flex flex-col gap-4"
          >
            {/* EMAIL */}
            <div>
              <label className="block mb-1">Email</label>
              <input
                type="email"
                {...register("email")}
                className="w-full p-2 rounded border"
              />

              {formErrors.email && (
                <p className="text-red-500 text-sm">
                  {formErrors.email.message}
                </p>
              )}
            </div>

            <div>
              <label className="block mb-1">Password</label>
              <input
                type="password"
                {...register("password")}
                className="w-full p-2 rounded border"
              />

              {formErrors.password && (
                <p className="text-red-500 text-sm">
                  {formErrors.password.message}
                </p>
              )}
            </div>

            <GreenButton
              type="submit"
              className="w-full py-2 text-lg"
              disabled={isLoading}
            >
              {isLoading ? "Logging in..." : "Login"}
            </GreenButton>
            <GreenButton
              className="w-full py-2 text-lg"
              onClick={() => navigate("/register")}
            >
              Registration
            </GreenButton>
          </form>
        </div>
      </div>
    </div>
  );
}
