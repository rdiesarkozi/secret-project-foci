import { useEffect, useState } from "react";

export function useTheme() {
    const [isDark, setIsDark] = useState(() => {
        const saved = localStorage.getItem("theme");
        return saved ? saved === "dark" : true;
    });

    useEffect(() => {
        const root = document.documentElement;
        if (isDark) {
            root.classList.remove("light-mode");
            localStorage.setItem("theme", "dark");
        } else {
            root.classList.add("light-mode");
            localStorage.setItem("theme", "light");
        }
    }, [isDark]);

    const toggleTheme = () => setIsDark((prev) => !prev);

    return {isDark, toggleTheme};
}