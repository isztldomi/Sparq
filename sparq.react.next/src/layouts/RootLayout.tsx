import { Outlet } from "react-router-dom";
import { Header } from "@/components/headers/Header";
import { RootContainer } from "@/components/containers/RootContainer";
import { Navbar } from "@/components/navbars/Navbar";

export function RootLayout() {
  return (
    <div className="min-h-screen flex flex-col">
      <Header />

      <main className="flex-1">
        <RootContainer>
          <Outlet />
        </RootContainer>
      </main>

      <Navbar />
    </div>
  );
}
