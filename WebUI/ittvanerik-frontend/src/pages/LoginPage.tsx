import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login as loginApi } from "../api/authApi";
import { useAuth } from "../context/AuthContext";
import "./LoginPage.css";

export default function LoginPage() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();
    const { login } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");

        try {
            setLoading(true);

            const response = await loginApi({ username, password });
            const token = response?.token;

            if (!token) {
                throw new Error("Login succeeded but no token was returned");
            }

            login(token);
            navigate("/matches", { replace: true });
        } catch (err: any) {
            setError(err.message || "Login failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-page">
            <div className="login-page__bg-circle login-page__bg-circle--top" />
            <div className="login-page__bg-circle login-page__bg-circle--bottom" />

            <div className="login-page__card">
                <section className="login-page__hero">
                    <div>
                        <div className="login-page__brand">
                            <div className="login-page__brand-icon">⚽</div>
                            <div>
                                <div className="login-page__brand-label">
                                    Football Prediction
                                </div>
                                <div className="login-page__brand-name">TipZone</div>
                            </div>
                        </div>

                        <div className="login-page__hero-copy">
                            <div className="login-page__eyebrow">
                                Predict \. Compete \. Win
                            </div>
                            <h1 className="login-page__title">
                                Sign in and make your next winning prediction
                            </h1>
                            <p className="login-page__description">
                                A sleek football tipping experience for match lovers.
                                Track fixtures, submit your score tips, and climb the
                                leaderboard with every round.
                            </p>
                        </div>
                    </div>

                    <div className="login-page__features">
                        <div className="login-page__feature">
                            <div className="login-page__feature-icon">🏆</div>
                            <div>
                                <div className="login-page__feature-title">
                                    Weekly challenges
                                </div>
                                <div className="login-page__feature-text">
                                    Compete with friends and earn points every matchday.
                                </div>
                            </div>
                        </div>

                        <div className="login-page__feature">
                            <div className="login-page__feature-icon">🎯</div>
                            <div>
                                <div className="login-page__feature-title">
                                    Fast score tips
                                </div>
                                <div className="login-page__feature-text">
                                    Clean, intuitive predictions with a modern matchday feel.
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="login-page__hero-ball">⚽</div>
                </section>

                <section className="login-page__form-panel">
                    <div className="login-page__form-wrap">
                        <div className="login-page__form-header">
                            <div className="login-page__form-eyebrow">Welcome back</div>
                            <h2 className="login-page__form-title">
                                Log in to your account
                            </h2>
                            <p className="login-page__form-description">
                                Enter your details to continue your football predictions.
                            </p>
                        </div>

                        <form onSubmit={handleSubmit}>
                            <div className="login-page__fields">
                                <div>
                                    <label className="login-page__label">Username</label>
                                    <input
                                        className="login-page__input"
                                        type="text"
                                        value={username}
                                        onChange={(e) => setUsername(e.target.value)}
                                        placeholder="Enter your username"
                                    />
                                </div>

                                <div>
                                    <label className="login-page__label">Password</label>
                                    <input
                                        className="login-page__input"
                                        type="password"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        placeholder="Enter your password"
                                    />
                                    <a href="/forgot-password" className="login-page__forgot-password">
                                        Forgot password?
                                    </a>
                                </div>

                                {error && (
                                    <div className="login-page__error">{error}</div>
                                )}

                                <button
                                    type="submit"
                                    disabled={loading || !username || !password}
                                    className="login-page__button"
                                >
                                    {loading ? "Signing in..." : "Log In"}
                                </button>
                                <div className="login-page__register">
                                    <span className="login-page__register-text">
                                        Don’t have an account?
                                    </span>
                                    <a href="/register" className="login-page__register-link">
                                        Register Here
                                    </a>
                                </div>
                            </div>
                            
                        </form>

                        <div className="login-page__footer">
                            Ready for the next matchday?
                        </div>
                    </div>
                </section>
            </div>
        </div>
    );
}
