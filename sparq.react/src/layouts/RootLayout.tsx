import { Outlet } from "react-router-dom";
import { Container } from "@/components/containers/Container";
import { Header } from "@/components/headers/Header";
import { Navbar } from "@/components/navbars/Navbar";

export function RootLayout() {
  return (
    <>
      <Header />
      <Navbar />
      <Container className="my-4">
        <Outlet />
      </Container>
    </>
  );
}
