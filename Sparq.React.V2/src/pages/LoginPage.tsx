import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { useAppDispatch } from "@/app/hooks";
import { login } from "@/features/auth/auth.thunks";
import { flattenErrors } from "@/api/errors/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import { HttpError } from "@/api/errors/HttpError";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginFormData } from "@/schemas/auth/login.schema";

export function LoginPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  // 👉 server errors marad külön
  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  // 👉 react-hook-form
  const {
    register,
    handleSubmit,
    formState: { errors: formErrors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  // 👉 submit
  const onSubmit = async (data: LoginFormData) => {
    setErrors([]);

    try {
      await dispatch(login(data)).unwrap();
      navigate("/profile");
    } catch (err: unknown) {
      if (err instanceof HttpError) setErrors(flattenErrors(err.errors));
    }
  };

  return (
    <div className="min-h-screen justify-center p-4">
      <h1>Login</h1>

      {/* 🔥 SERVER ERRORS */}
      <ErrorsContainer errors={errors} />

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

            {/* PASSWORD */}
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

            <GreenButton type="submit" className="w-full py-2 text-lg">
              Login
            </GreenButton>
          </form>
        </div>
      </div>
    </div>
  );
}
