import { NavLink } from "react-router-dom";
import styles from "./Navbar.module.css";

interface Props {
  to: string;
  icon: React.ReactNode;
  label: string;
  collapsed?: boolean;
}

export function NavbarLink({ to, icon, label, collapsed }: Props) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) => `
        flex items-center gap-3 px-3 py-2 rounded-md
        transition-all

        ${styles.navItem}
        ${!isActive ? styles.navItemHover : styles.navItemActive}
      `}
    >
      <span className="text-lg">{icon}</span>

      {!collapsed && <span className="text-sm">{label}</span>}
    </NavLink>
  );
}
