import { Outlet } from "react-router-dom";
import { Container } from "@/components/containers/Container";
import { Header } from "@/components/headers/Header";

export function SimpleLayout() {
  return (
    <>
      <Header />
      <div className="flex flex-1">
        <main className="flex-1">
          <Container>
            <Outlet />
          </Container>
        </main>
      </div>
    </>
  );
}
