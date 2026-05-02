import { FlaskConical, CircleFadingPlus, Award, User } from "lucide-react";

import { NavbarLink } from "@/components/navbars/NavbarLink";

export function Navbar() {
  return (
    <nav className="bg-[var(--surface-2)] sticky bottom-0 z-50 shadow-md rounded-t-3xl py-1">
      <div className="flex justify-center gap-10 md:gap-20 lg:gap-30 xl:gap-40 py-3 max-w-md mx-auto">
        <NavbarLink
          to="/quizzes"
          icon={FlaskConical}
          activeColor="text-[var(--nav-icon-1)]"
        />
        <NavbarLink
          to="/my-quizzes"
          icon={CircleFadingPlus}
          activeColor="text-[var(--nav-icon-2)]"
        />
        <NavbarLink
          to="/history"
          icon={Award}
          activeColor="text-[var(--nav-icon-3)]"
        />
        <NavbarLink
          to="/profile"
          icon={User}
          activeColor="text-[var(--nav-icon-4)]"
        />
      </div>
    </nav>
  );
}
