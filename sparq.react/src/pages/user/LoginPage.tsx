import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import type { ChangeEvent, FormEvent } from "react";
import {
  loginSchema,
  type LoginRequestDto,
} from "@/api/models/loginDto/LoginRequestDto";
import { useUserContext } from "@/contexts/UserContext";
import { FormError } from "@/components/FormError";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";
import { ServerSideValidationError } from "@/api/errors/ServerSideValidationError";
import { HttpError } from "@/api/errors/HttpError";
import { ZodError, type ZodIssue } from "zod";

export function LoginPage() {
  const userContext = useUserContext();
  const navigate = useNavigate();
  const location = useLocation();

  const [loading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [loginData, setLoginData] = useState<LoginRequestDto>({
    email: "",
    password: "",
  });

  const [formErrors, setFormErrors] = useState<Record<string, string>>({});

  function handleInputChange(e: ChangeEvent<HTMLInputElement>) {
    setLoginData((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  }

  async function handleFormSubmit(evt: FormEvent<HTMLFormElement>) {
    evt.preventDefault();

    setError(null);
    setFormErrors({});
    setIsLoading(true);

    try {
      loginSchema.parse(loginData);

      await userContext.handleLogin(loginData);

      if (location.state?.loginRedirect) {
        navigate(location.state.loginRedirect);
      } else {
        navigate("/");
      }
    } catch (e: unknown) {
      if (e instanceof ZodError) {
        const errors: Record<string, string> = {};

        e.issues.forEach((err: ZodIssue) => {
          const path = err.path[0];

          if (typeof path === "string") {
            errors[path] = err.message;
          }
        });

        setFormErrors(errors);
      } else if (e instanceof ServerSideValidationError) {
        setFormErrors(e.validationErrors);
      } else if (e instanceof HttpError) {
        setError(e.message);
      } else {
        setError("Unknown error");
      }
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow rounded-lg">
      {error && <ErrorAlert message={error} />}

      <h1 className="text-2xl font-bold mb-6">Login</h1>

      <form onSubmit={handleFormSubmit} noValidate className="space-y-4">
        <div>
          <label className="block text-sm font-medium">Email</label>
          <input
            type="text"
            name="email"
            value={loginData.email}
            onChange={handleInputChange}
            className="mt-1 w-full border rounded px-3 py-2"
          />
          <FormError message={formErrors.email} />
        </div>

        <div>
          <label className="block text-sm font-medium">Password</label>
          <input
            type="password"
            name="password"
            value={loginData.password}
            onChange={handleInputChange}
            className="mt-1 w-full border rounded px-3 py-2"
          />
          <FormError message={formErrors.password} />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded disabled:opacity-50"
        >
          {loading ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
}
