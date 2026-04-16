import { Link, useLocation } from "react-router-dom";
import ThemeToggle from "./ThemeToggle";
import {useAuth} from "../context/AuthContext.tsx";

const navItems = [
    { to: "/", label: "Home" },
    { to: "/matches", label: "Matches" },
    { to: "/my-tips", label: "My Tips" },
    { to: "/groups", label: "My Groups" },
    { to: "/profile", label: "Profile" },
];

export default function NavigationMenu({ className = "group-page" }: { className?: string }) {
    const location = useLocation();
    const { isAuthenticated, logout } = useAuth();

    const handleLogout = () => {
        logout();
    };

    return (
        <nav className={`${className}__nav`}>
            {navItems.map((item) => {
                const isActive = location.pathname === item.to;
                const linkClass = isActive
                    ? `${className}__nav-link ${className}__nav-link--active`
                    : `${className}__nav-link`;

                return (
                    <Link key={item.to} to={item.to} className={linkClass}>
                        {item.label}
                    </Link>
                );
            })}
            <ThemeToggle />
            {isAuthenticated ? (
                <button type="button" onClick={handleLogout} className="home-page__nav-button">
                    Log out
                </button>
            ) : (
                <Link to="/login" className="home-page__nav-button">Login</Link>
            )}
        </nav>
    );
}