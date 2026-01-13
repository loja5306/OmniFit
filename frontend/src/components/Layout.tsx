import React from "react";
import { Link, Outlet } from "react-router";

const Layout = () => {
  return (
    <div className="min-h-screen bg-slate-100">
      <nav className="w-full flex justify-between items-center p-4 bg-blue-400 text-white">
        <Link to="/" className="text-2xl font-bold">
          OmniFit
        </Link>
        <div className="space-x-4">
          <Link to="/workouts" className="">
            Workouts
          </Link>
          <Link to="/exercises" className="">
            Exercises
          </Link>
        </div>
      </nav>
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
