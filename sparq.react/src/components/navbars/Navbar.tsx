import { NavbarLink } from "@/components/navbars/NavbarLink";

export function Navbar() {
  return (
    <nav className="w-full border-b bg-white">
      <div className="mx-auto flex h-12 max-w-6xl items-center gap-6 px-4">
        <NavbarLink to="/">Home</NavbarLink>
        <NavbarLink to="/dashboard">Dashboard</NavbarLink>
        <NavbarLink to="/about">About</NavbarLink>
      </div>
    </nav>
  );
}
