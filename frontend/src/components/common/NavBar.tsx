import { Link } from "react-router";
import { useAuth } from "../../contexts/AuthContext";
import logo from "../../assets/omnifit-logo.svg";

const NavBar = () => {
  const { isAuthenticated, email, logout } = useAuth();

  return (
    <nav className="w-full flex justify-between items-center p-4 bg-white shadow-sm text-primary font-semibold text-lg">
      <div className="flex justify-center items-center gap-4">
        <Link to="/" className="text-2xl font-bold">
          <img src={logo} alt="OmniFit" height={2} />
        </Link>
        <div className="w-px h-10 bg-gray-200" />
        <div className="flex items-center space-x-4">
          <Link to="/workouts">Workouts</Link>
          <Link to="/exercises">Exercises</Link>
        </div>
      </div>
      <div className="border border-gray-200 shadow-sm rounded-lg px-4 py-2 space-x-4">
        {isAuthenticated ? (
          <>
            <span className="font-medium">{email}</span>
            <button onClick={logout} className="cursor-pointer hover:underline">
              Logout
            </button>
          </>
        ) : (
          <>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default NavBar;
