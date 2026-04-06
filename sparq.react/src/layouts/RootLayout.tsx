import { Outlet } from "react-router-dom";
import { Container } from "@/components/containers/Container";
import { Header } from "@/components/headers/Header";
import { Navbar } from "@/components/navbars/Navbar";

export function RootLayout() {
  return (
    <>
      <Header />
      <div className="flex flex-1">
        <Navbar />
        <main className="flex-1">
          <Container>
            <Outlet />
          </Container>
        </main>
      </div>
    </>
  );
}
