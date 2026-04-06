import { Outlet } from "react-router-dom";
import { Container } from "@/components/containers/Container";
import { Header } from "@/components/headers/Header";

export function SimpleLayout() {
  return (
    <>
      <Header />
      <Container className="my-4">
        <Outlet />
      </Container>
    </>
  );
}
