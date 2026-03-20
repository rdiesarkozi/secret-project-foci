import { Link } from "react-router-dom";
import "./MatchesPage.css";

const matches = [
    {
        id: 1,
        homeTeam: "Manchester City",
        awayTeam: "Liverpool",
        date: "Sat, 16 Mar",
        time: "18:30",
        status: "Open",
    },
    {
        id: 2,
        homeTeam: "Arsenal",
        awayTeam: "Chelsea",
        date: "Sun, 17 Mar",
        time: "15:00",
        status: "Open",
    },
    {
        id: 3,
        homeTeam: "Barcelona",
        awayTeam: "Real Madrid",
        date: "Sun, 17 Mar",
        time: "21:00",
        status: "Closing Soon",
    },
];

export default function MatchesPage() {
    return (
        <div className="matches-page">
            <div className="matches-page__bg-circle matches-page__bg-circle--top" />
            <div className="matches-page__bg-circle matches-page__bg-circle--bottom" />

            <header className="matches-page__header">
                <div className="matches-page__brand">
                    <div className="matches-page__brand-icon">⚽</div>
                    <div>
                        <div className="matches-page__brand-label">Football Prediction</div>
                        <div className="matches-page__brand-name">TipZone</div>
                    </div>
                </div>

                <nav className="matches-page__nav">
                    <Link to="/" className="matches-page__nav-link">Home</Link>
                    <Link to="/matches" className="matches-page__nav-link matches-page__nav-link--active">
                        Matches
                    </Link>
                    <Link to="/leaderboard" className="matches-page__nav-link">Leaderboard</Link>
                    <Link to="/my-tips" className="matches-page__nav-link">My Tips</Link>
                    <Link to="/profile" className="matches-page__nav-link">Profile</Link>
                </nav>
            </header>

            <main className="matches-page__content">
                <section className="matches-page__hero">
                    <div className="matches-page__hero-copy">
                        <div className="matches-page__eyebrow">Upcoming Fixtures</div>
                        <h1 className="matches-page__title">
                            Make your predictions for the next matchday
                        </h1>
                        <p className="matches-page__description">
                            Browse the latest fixtures, check kickoff times, and submit
                            your score tips in a clean matchday dashboard.
                        </p>
                    </div>

                    <div className="matches-page__summary-card">
                        <div className="matches-page__summary-badge">Matchday Overview</div>
                        <div className="matches-page__summary-stat">
                            <span className="matches-page__summary-value">3</span>
                            <span className="matches-page__summary-label">Open Matches</span>
                        </div>
                        <div className="matches-page__summary-stat">
                            <span className="matches-page__summary-value">1</span>
                            <span className="matches-page__summary-label">Closing Soon</span>
                        </div>
                    </div>
                </section>

                <section className="matches-page__list">
                    {matches.map((match) => (
                        <article key={match.id} className="matches-page__card">
                            <div className="matches-page__card-top">
                                <span className="matches-page__date">
                                    {match.date} {match.time}
                                </span>
                                <span
                                    className={`matches-page__status ${
                                        match.status === "Closing Soon"
                                            ? "matches-page__status--warning"
                                            : ""
                                    }`}
                                >
                                    {match.status}
                                </span>
                            </div>

                            <div className="matches-page__teams">
                                <div className="matches-page__team__home">{match.homeTeam}</div>
                                <div className="matches-page__vs">VS</div>
                                <div className="matches-page__team__away">{match.awayTeam}</div>
                            </div>

                            <div className="matches-page__actions">
                                <button className="matches-page__button">Add Prediction</button>
                            </div>
                        </article>
                    ))}
                </section>
            </main>
        </div>
    );
}
