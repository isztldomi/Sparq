import { useEffect, useState } from "react";
import { HeaderLink } from "@/components/headers/HeaderLink";

export function Header() {
  const [hidden, setHidden] = useState(false);
  const [lastScrollY, setLastScrollY] = useState(0);

  useEffect(() => {
    const handleScroll = () => {
      const currentScrollY = window.scrollY;

      if (currentScrollY > lastScrollY && currentScrollY > 50) {
        // lefelé scroll → elrejt
        setHidden(true);
      } else {
        // felfelé scroll → azonnal vissza
        setHidden(false);
      }

      setLastScrollY(currentScrollY);
    };

    window.addEventListener("scroll", handleScroll);

    return () => window.removeEventListener("scroll", handleScroll);
  }, [lastScrollY]);

  return (
    <div
      className={`w-full py-5 px-5 sticky top-0 z-50 transition-transform duration-300 ${
        hidden ? "-translate-y-full" : "translate-y-0"
      }`}
    >
      <header className="bg-[var(--surface-2)] text-center shadow-md rounded-3xl py-2">
        <HeaderLink
          to="/"
          label="SparQ"
          activeColor="text-[var(--header-text)]"
        />
      </header>
    </div>
  );
}
