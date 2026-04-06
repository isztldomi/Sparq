import { useState } from "react";
import styles from "./Navbar.module.css";
import { NavbarLink } from "@/components/navbars/NavbarLink";

export function Navbar() {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <aside
      className={`
        flex flex-col transition-all duration-300
        ${styles.navbar}
        ${collapsed ? "w-16 min-w-16" : "w-56 min-w-56"}
      `}
    >
      {/* NAV ITEMS */}
      <nav className="flex flex-col gap-1 p-3 flex-1">
        <NavbarLink to="/" icon="🏠" label="Home" collapsed={collapsed} />

        <NavbarLink
          to="/dashboard"
          icon="📊"
          label="Dashboard"
          collapsed={collapsed}
        />

        <NavbarLink to="/about" icon="ℹ️" label="About" collapsed={collapsed} />
      </nav>

      {/* TOGGLE */}
      <button
        onClick={() => setCollapsed(!collapsed)}
        className={`
          p-3 text-sm transition
          ${styles.toggleButton}
        `}
      >
        {collapsed ? "➡️" : "⬅️ Collapse"}
      </button>
    </aside>
  );
}
