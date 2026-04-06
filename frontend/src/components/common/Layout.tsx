import { Outlet } from "react-router";
import NavBar from "./NavBar";

const Layout = () => {

  return (
    <div className="min-h-screen bg-slate-100">
      <NavBar />
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
