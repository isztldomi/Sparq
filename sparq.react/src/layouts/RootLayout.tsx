import { Outlet } from "react-router-dom";
import { Container } from "@/components/containers/Container";
import { Header } from "@/components/headers/Header";
import { Navbar } from "@/components/navbars/Navbar";

export function RootLayout() {
  return (
    <>
      <Header />

      <div className="pt-[var(--header-height)] grid grid-cols-[240px_1fr] min-h-screen">
        <Navbar />

        <Container>
          <Outlet />
        </Container>
      </div>
    </>
  );
}
