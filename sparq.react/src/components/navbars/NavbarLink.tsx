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
        flex items-center rounded-md transition-all
        no-underline
        px-3 py-2

        ${collapsed ? "justify-center" : "gap-3"}
        ${styles.navItem}
        ${isActive ? styles.navItemActive : styles.navItemHover}
      `}
    >
      <span className="text-lg">{icon}</span>

      {!collapsed && <span className="text-sm">{label}</span>}
    </NavLink>
  );
}
