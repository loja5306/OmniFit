import { Link } from "react-router"
import { useAuth } from "../contexts/AuthContext";

const NavBar = () => {
  const { isAuthenticated, email, logout } = useAuth();

  return (
    <nav className="w-full flex justify-between items-center p-4 bg-blue-400 text-white">
        <div className="flex justify-center items-center gap-4">
          <Link to="/" className="text-2xl font-bold">
            OmniFit
          </Link>
          <div className="w-px h-8 bg-white/50" />
          <div className="flex items-center space-x-4">
            <Link to="/workouts">Workouts</Link>
            <Link to="/exercises">Exercises</Link>
          </div>
        </div>
        <div className="border border-white/50 rounded-lg px-4 py-2 space-x-4">
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
  )
}

export default NavBar
