// src/pages/RegisterPage.tsx
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./RegisterPage.css";
import { register } from "../api/authApi.ts";

export default function RegisterPage() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");

        if (!username.trim() || !email.trim() || !password || !confirmPassword) {
            setError("All fields are required");
            return;
        }

        if (password !== confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        try {
            setLoading(true);

            await register({
                username: username.trim(),
                email: email.trim(),
                password,
            });

            navigate("/login", { replace: true });
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : "Registration failed";
            setError(message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="register-page">
            <div className="register-page__bg-circle register-page__bg-circle--top" />
            <div className="register-page__bg-circle register-page__bg-circle--bottom" />

            <div className="register-page__card">
                <section className="register-page__hero">
                    <div>
                        <div className="register-page__brand">
                            <div className="register-page__brand-icon">⚽</div>
                            <div>
                                <div className="register-page__brand-label">Football Prediction</div>
                                <div className="register-page__brand-name">TipZone</div>
                            </div>
                        </div>

                        <div className="register-page__hero-copy">
                            <div className="register-page__eyebrow">Predict . Compete . Win</div>
                            <h1 className="register-page__title">
                                Create your account and join the next matchday
                            </h1>
                            <p className="register-page__description">
                                Sign up to submit predictions, follow fixtures, and
                                compete on the leaderboard in a modern football tipping experience.
                            </p>
                        </div>
                    </div>

                    <div className="register-page__features">
                        <div className="register-page__feature">
                            <div className="register-page__feature-icon">⚡</div>
                            <div>
                                <div className="register-page__feature-title">Quick setup</div>
                                <div className="register-page__feature-text">
                                    Create your profile in moments and start predicting.
                                </div>
                            </div>
                        </div>

                        <div className="register-page__feature">
                            <div className="register-page__feature-icon">📈</div>
                            <div>
                                <div className="register-page__feature-title">Climb the rankings</div>
                                <div className="register-page__feature-text">
                                    Earn points every round and challenge other players.
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="register-page__hero-ball">⚽</div>
                </section>

                <section className="register-page__form-panel">
                    <div className="register-page__form-wrap">
                        <div className="register-page__form-header">
                            <div className="register-page__form-eyebrow">Join TipZone</div>
                            <h2 className="register-page__form-title">Create your account</h2>
                            <p className="register-page__form-description">
                                Enter your details to start your football prediction journey.
                            </p>
                        </div>

                        <form onSubmit={handleSubmit}>
                            <div className="register-page__fields">
                                <div>
                                    <label className="register-page__label">Username</label>
                                    <input
                                        className="register-page__input"
                                        type="text"
                                        value={username}
                                        onChange={(e) => setUsername(e.target.value)}
                                        placeholder="Choose a username"
                                    />
                                </div>

                                <div>
                                    <label className="register-page__label">Email</label>
                                    <input
                                        className="register-page__input"
                                        type="email"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        placeholder="Enter your email"
                                    />
                                </div>

                                <div>
                                    <label className="register-page__label">Password</label>
                                    <input
                                        className="register-page__input"
                                        type="password"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        placeholder="Create a password"
                                    />
                                </div>

                                <div>
                                    <label className="register-page__label">Confirm Password</label>
                                    <input
                                        className="register-page__input"
                                        type="password"
                                        value={confirmPassword}
                                        onChange={(e) => setConfirmPassword(e.target.value)}
                                        placeholder="Confirm your password"
                                    />
                                </div>

                                {error && <div className="register-page__error">{error}</div>}

                                <button
                                    type="submit"
                                    disabled={loading}
                                    className="register-page__button"
                                >
                                    {loading ? "Creating account..." : "Register"}
                                </button>

                                <div className="register-page__login">
                                    <span className="register-page__login-text">
                                        Already have an account?
                                    </span>
                                    <Link to="/login" className="register-page__login-link">
                                        Back to login
                                    </Link>
                                </div>
                            </div>
                        </form>

                        <div className="register-page__footer">
                            Your next winning prediction starts here.
                        </div>
                    </div>
                </section>
            </div>
        </div>
    );
}
