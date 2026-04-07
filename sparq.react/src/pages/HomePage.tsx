import { useEffect, useState } from "react";

export function HomePage() {
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsLoading(false);
    }, 500);

    return () => clearTimeout(timer);
  }, []);

  if (isLoading) {
    return (
      <div
        style={{
          color: "var(--color-text-secondary)",
          textAlign: "center",
          marginTop: "2rem",
        }}
      >
        Loading...
      </div>
    );
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
      <h1 style={{ color: "var(--color-text-primary)" }}>Welcome</h1>

      <p style={{ color: "var(--color-text-secondary)" }}>
        This is the home page.
      </p>

      {/* Card példa */}
      <div className="card">
        <h2>Example Card</h2>
        <p>This uses your global styles.</p>
      </div>

      {/* Button példa */}
      <button className="button-primary">Click me</button>

      {/* Status példák */}
      <div style={{ display: "flex", gap: "1rem", flexWrap: "wrap" }}>
        <span
          style={{
            color: "var(--color-success-text)",
            background: "var(--color-success-bg)",
            padding: "0.25rem 0.5rem",
            borderRadius: "0.5rem",
            border: "1px solid var(--color-success-border)",
          }}
        >
          Success
        </span>

        <span
          style={{
            color: "var(--color-error-text)",
            background: "var(--color-error-bg)",
            padding: "0.25rem 0.5rem",
            borderRadius: "0.5rem",
            border: "1px solid var(--color-error-border)",
          }}
        >
          Error
        </span>

        <span
          style={{
            color: "var(--color-warning-text)",
            background: "var(--color-warning-bg)",
            padding: "0.25rem 0.5rem",
            borderRadius: "0.5rem",
            border: "1px solid var(--color-warning-border)",
          }}
        >
          Warning
        </span>
      </div>
    </div>
  );
}
