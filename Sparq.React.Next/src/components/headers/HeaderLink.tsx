import { NavLink } from "react-router-dom";

interface HeaderLinkProps {
  to: string;
  label: string;
  activeColor: string;
}

export function HeaderLink({ to, label, activeColor }: HeaderLinkProps) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        `transition-colors duration-300 ${
          isActive ? activeColor : `text-[var(--navLink)]`
        }`
      }
    >
      <h1>{label}</h1>
    </NavLink>
  );
}
