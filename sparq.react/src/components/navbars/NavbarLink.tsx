import { NavLink } from "react-router-dom";

interface Props {
  to: string;
  children: React.ReactNode;
}

export function NavbarLink({ to, children }: Props) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        isActive
          ? "text-blue-600 font-semibold"
          : "text-gray-600 hover:text-blue-500 transition-colors"
      }
    >
      {children}
    </NavLink>
  );
}
