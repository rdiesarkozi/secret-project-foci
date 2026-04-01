import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import PrivateRoute from "./router/PrivateRoute";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import MatchesPage from "./pages/MatchesPage";
import ProfilePage from "./pages/ProfilePage";
import LeaderboardPage from "./pages/LeaderboardPage.tsx";
import MyTipsPage from "./pages/MyTipsPage";
import GroupPage from "./pages/GroupPage.tsx";

function ForgotPasswordPage() {
    return <div>Forgot Password Page</div>;
}

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route path="/forgot-password" element={<ForgotPasswordPage />} />

                    <Route element={<PrivateRoute />}>
                        <Route path="/matches" element={<MatchesPage />} />
                        <Route path="/leaderboard" element={<LeaderboardPage />} />
                        <Route path="/profile" element={<ProfilePage />} />
                        <Route path="/my-tips" element={<MyTipsPage />} />
                        <Route path="/groups" element={<GroupPage />} />
                    </Route>

                    <Route path="*" element={<h1>Page not found</h1>} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}

export default App;
