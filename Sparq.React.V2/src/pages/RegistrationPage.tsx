import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { GreenButton } from "@/components/buttons/greenButton";
import { useAppDispatch } from "@/app/hooks";
import { login, register } from "@/features/auth/auth.thunks";
import { flattenErrors } from "@/api/errors/flattenErrors";
import { ErrorsContainer } from "@/components/errors/ErrorsContainer";
import type { ProblemDetails } from "@/api/models/ProblemDetails";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  registerSchema,
  type RegisterFormData,
} from "@/schemas/auth/register.schema";

export function RegistrationPage() {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const [errors, setErrors] = useState<{ field: string; message: string }[]>(
    [],
  );

  const {
    register: formRegister,
    handleSubmit,
    formState: { errors: formErrors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  const onSubmit = async (data: RegisterFormData) => {
    setErrors([]);

    try {
      await dispatch(register(data)).unwrap();

      await dispatch(
        login({
          email: data.email,
          password: data.password,
        }),
      ).unwrap();

      navigate("/profile");
    } catch (err: unknown) {
      const error = err as ProblemDetails;

      setErrors(flattenErrors(error.errors));
    }
  };

  //const onSubmit = async (data: RegisterFormData) => {
  //  setErrors([]);
  //
  //  try {
  //    await dispatch(register(data)).unwrap();
  //
  //    await dispatch(
  //      login({
  //        email: data.email,
  //        password: data.password,
  //      }),
  //    ).unwrap();
  //
  //    navigate("/profile");
  //  } catch (err: unknown) {
  //    if (err instanceof HttpError) setErrors(flattenErrors(err.errors));
  //  }
  //};

  return (
    <div className="min-h-screen justify-center p-4">
      <h1>Registration</h1>

      <ErrorsContainer errors={errors} />

      <div className="flex justify-center">
        <div className="sm:min-w-[200px] md:min-w-[400px] bg-[var(--surface-4)] p-6 rounded-lg shadow-md">
          <form
            onSubmit={handleSubmit(onSubmit)}
            className="flex flex-col gap-4"
          >
            {/* FIRST NAME */}
            <div>
              <label className="block mb-1">First Name</label>
              <input
                {...formRegister("firstName")}
                className="w-full p-2 rounded border"
              />
              {formErrors.firstName && (
                <p className="text-red-500 text-sm">
                  {formErrors.firstName.message}
                </p>
              )}
            </div>

            {/* LAST NAME */}
            <div>
              <label className="block mb-1">Last Name</label>
              <input
                {...formRegister("lastName")}
                className="w-full p-2 rounded border"
              />
              {formErrors.lastName && (
                <p className="text-red-500 text-sm">
                  {formErrors.lastName.message}
                </p>
              )}
            </div>

            {/* NICKNAME */}
            <div>
              <label className="block mb-1">Nick Name</label>
              <input
                {...formRegister("nickName")}
                className="w-full p-2 rounded border"
              />
              {formErrors.nickName && (
                <p className="text-red-500 text-sm">
                  {formErrors.nickName.message}
                </p>
              )}
            </div>

            {/* EMAIL */}
            <div>
              <label className="block mb-1">Email</label>
              <input
                type="email"
                {...formRegister("email")}
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
                {...formRegister("password")}
                className="w-full p-2 rounded border"
              />
              {formErrors.password && (
                <p className="text-red-500 text-sm">
                  {formErrors.password.message}
                </p>
              )}
            </div>

            <GreenButton type="submit" className="w-full py-2 text-lg">
              Registration
            </GreenButton>
          </form>
        </div>
      </div>
    </div>
  );
}
