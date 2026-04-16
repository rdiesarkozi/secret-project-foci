import { Link } from "react-router-dom";
import "./HomePage.css";
import NavigationMenu from "../components/NavigationMenu.tsx";

export default function HomePage() {

    return (
        <div className="home-page">
            <div className="home-page__bg-circle home-page__bg-circle--top" />
            <div className="home-page__bg-circle home-page__bg-circle--bottom" />

            <header className="home-page__header">
                <Link to="/">
                <div className="home-page__brand">
                    <div className="home-page__brand-icon">⚽</div>
                    <div className="home-page__brand-text">
                        <div className="home-page__brand-label">Football Prediction</div>
                        <div className="home-page__brand-name">TipZone</div>
                    </div>
                </div>
                </Link>

                <nav className="home-page__nav">
                   <NavigationMenu />
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
