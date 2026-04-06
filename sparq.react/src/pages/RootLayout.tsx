import { Outlet } from "react-router-dom";

export function RootLayout() {
  return (
    <div className="flex min-h-screen bg-[var(--color-bg)] text-[var(--color-text-primary)]">
      {/* Sidebar */}
      <aside className="hidden md:flex w-64 flex-col bg-[var(--color-bg-card)] p-4">
        <h2 className="text-lg font-bold mb-4">Menu</h2>
        <nav className="flex flex-col gap-2">
          <a href="/" className="hover:text-[var(--color-brand)]">
            Home
          </a>
        </nav>
      </aside>

      {/* Main area */}
      <div className="flex flex-col flex-1">
        {/* Header */}
        <header className="border-b border-gray-700 p-4">
          <h1 className="text-xl font-semibold">My App</h1>
        </header>

        {/* Content */}
        <main className="flex-1 px-4 py-4 border border-red-500">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
