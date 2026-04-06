import { Outlet } from "react-router-dom";

export function RootLayout() {
  return (
    <>
      <header style={{ padding: "1rem", borderBottom: "1px solid #ccc" }}>
        <h1>My App</h1>
      </header>

      <main style={{ padding: "1rem" }}>
        <Outlet />
      </main>
    </>
  );
}
