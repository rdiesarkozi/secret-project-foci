import { useTheme } from "../hooks/useTheme";

export default function ThemeToggle() {
    const {isDark, toggleTheme} = useTheme();

    return (
        <button
            onClick={toggleTheme}
            className="theme-toggle"
            aria-label={isDark ? "Switch to light mode" : "Switch to dark mode"}
            title={isDark ? "Light mode" : "Dark mode"}
        >
            {isDark ? "☀️" : "🌙"}
        </button>
    )
};