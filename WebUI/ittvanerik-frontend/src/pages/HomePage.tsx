import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import "./HomePage.css";

export default function HomePage() {
    const { logout, isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login", { replace: true });
    };

    return (
        <div className="home-page">
            <div className="home-page__bg-circle home-page__bg-circle--top" />
            <div className="home-page__bg-circle home-page__bg-circle--bottom" />

            <header className="home-page__header">
                <div className="home-page__brand">
                    <div className="home-page__brand-icon">⚽</div>
                    <div className="home-page__brand-text">
                        <div className="home-page__brand-label">Football Prediction</div>
                        <div className="home-page__brand-name">TipZone</div>
                    </div>
                </div>

                <nav className="home-page__nav">
                    <Link to="/matches" className="home-page__nav-link">Matches</Link>
                    <Link to="/leaderboard" className="home-page__nav-link">Leaderboard</Link>
                    <Link to="/my-tips" className="home-page__nav-link">My Tips</Link>
                    <Link to="/profile" className="home-page__nav-link">Profile</Link>
                    {isAuthenticated ? (
                        <button type="button" onClick={handleLogout} className="home-page__nav-button">
                            Log out
                        </button>
                    ) : (
                        <Link to="/login" className="home-page__nav-button">Login</Link>
                    )}
                </nav>
            </header>

            <main className="home-page__content">
                <section className="home-page__hero">
                    <div className="home-page__hero-copy">
                        <div className="home-page__eyebrow">Welcome to TipZone</div>
                        <h1 className="home-page__title">Your football prediction dashboard</h1>
                        <p className="home-page__description">
                            Follow upcoming fixtures, submit predictions, track results,
                            and compete with other players in a clean modern experience.
                        </p>

                        <div className="home-page__actions">
                            <Link to="/matches" className="home-page__primary-button">
                                View Matches
                            </Link>
                            <Link to="/leaderboard" className="home-page__secondary-button">
                                See Leaderboard
                            </Link>
                        </div>
                    </div>

                    <div className="home-page__hero-card">
                        <div className="home-page__card-badge">Next Matchday</div>
                        <h2 className="home-page__card-title">Make your predictions</h2>
                        <p className="home-page__card-text">
                            Stay ahead with quick access to fixtures, standings, and your latest tips.
                        </p>

                        <div className="home-page__stats">
                            <div className="home-page__stat">
                                <span className="home-page__stat-value">12</span>
                                <span className="home-page__stat-label">Open Matches</span>
                            </div>
                            <div className="home-page__stat">
                                <span className="home-page__stat-value">5</span>
                                <span className="home-page__stat-label">Predictions Made</span>
                            </div>
                            <div className="home-page__stat">
                                <span className="home-page__stat-value">3</span>
                                <span className="home-page__stat-label">Points This Week</span>
                            </div>
                        </div>
                    </div>
                </section>
            </main>
        </div>
    );
}
