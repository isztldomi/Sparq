import { NavLink } from "react-router-dom";
import type { LucideIcon } from "lucide-react";

type Props = {
  to: string;
  icon: LucideIcon;
  activeColor: string;
};

export function NavbarLink({ to, icon: Icon, activeColor }: Props) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        `transition-colors duration-300 ${
          isActive ? activeColor : "text-[var(--text)]"
        }`
      }
    >
      <Icon className="w-10 h-10" />
    </NavLink>
  );
}
