// src/context/AuthContext.tsx
import { createContext, useContext, useMemo, useState } from "react";

type AuthContextType = {
    user: string | null;
    token: string | null;
    isAuthenticated: boolean;
    login: (token: string, username?: string | null) => void;
    logout: () => void;
};

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(localStorage.getItem("token"));
    const [user, setUser] = useState<string | null>(localStorage.getItem("username"));

    const login = (nextToken: string, username?: string | null) => {
        localStorage.setItem("token", nextToken);
        setToken(nextToken);

        if (username) {
            localStorage.setItem("username", username);
            setUser(username);
        }
    };

    const logout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("username");
        setToken(null);
        setUser(null);
    };

    const value = useMemo(
        () => ({
            user,
            token,
            isAuthenticated: Boolean(token),
            login,
            logout,
        }),
        [user, token]
    );

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => useContext(AuthContext);
