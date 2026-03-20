import { Link } from "react-router-dom";
import "./LeaderboardPage.css";

const leaderboard = [
    { id: 1, name: "Alex Morgan", points: 128, rank: 1, change: "+2" },
    { id: 2, name: "Chris Taylor", points: 121, rank: 2, change: "-1" },
    { id: 3, name: "Jordan Lee", points: 119, rank: 3, change: "+1" },
    { id: 4, name: "Sam Carter", points: 112, rank: 4, change: "0" },
    { id: 5, name: "Jamie Brown", points: 108, rank: 5, change: "+3" },
];

export default function LeaderboardPage() {
    return (
        <div className="leaderboard-page">
            <div className="leaderboard-page__bg-circle leaderboard-page__bg-circle--top" />
            <div className="leaderboard-page__bg-circle leaderboard-page__bg-circle--bottom" />

            <header className="leaderboard-page__header">
                <div className="leaderboard-page__brand">
                    <div className="leaderboard-page__brand-icon">⚽</div>
                    <div>
                        <div className="leaderboard-page__brand-label">Football Prediction</div>
                        <div className="leaderboard-page__brand-name">TipZone</div>
                    </div>
                </div>

                <nav className="leaderboard-page__nav">
                    <Link to="/" className="leaderboard-page__nav-link">
                        Home
                    </Link>
                    <Link to="/matches" className="leaderboard-page__nav-link">
                        Matches
                    </Link>
                    <Link
                        to="/leaderboard"
                        className="leaderboard-page__nav-link leaderboard-page__nav-link--active"
                    >
                        Leaderboard
                    </Link>
                    <Link to="/my-tips" className="leaderboard-page__nav-link">
                        My Tips
                    </Link>
                    <Link to="/profile" className="leaderboard-page__nav-link">
                        Profile
                    </Link>
                </nav>
            </header>

            <main className="leaderboard-page__content">
                <section className="leaderboard-page__hero">
                    <div className="leaderboard-page__hero-copy">
                        <div className="leaderboard-page__eyebrow">Top Players</div>
                        <h1 className="leaderboard-page__title">
                            Follow the ranking and see who leads the matchday
                        </h1>
                        <p className="leaderboard-page__description">
                            Track the best predictors, compare scores, and watch the competition
                            change after every round.
                        </p>
                    </div>

                    <div className="leaderboard-page__summary-card">
                        <div className="leaderboard-page__summary-badge">Current Overview</div>
                        <div className="leaderboard-page__summary-stat">
                            <span className="leaderboard-page__summary-value">24</span>
                            <span className="leaderboard-page__summary-label">Active Players</span>
                        </div>
                        <div className="leaderboard-page__summary-stat">
                            <span className="leaderboard-page__summary-value">128</span>
                            <span className="leaderboard-page__summary-label">Top Score</span>
                        </div>
                    </div>
                </section>

                <section className="leaderboard-page__table-wrap">
                    <div className="leaderboard-page__table-header">
                        <h2 className="leaderboard-page__table-title">Leaderboard standings</h2>
                    </div>

                    <div className="leaderboard-page__table">
                        {leaderboard.map((player) => (
                            <article key={player.id} className="leaderboard-page__row">
                                <div className="leaderboard-page__rank">#{player.rank}</div>

                                <div className="leaderboard-page__player">
                                    <div className="leaderboard-page__avatar">
                                        {player.name.charAt(0)}
                                    </div>
                                    <div>
                                        <div className="leaderboard-page__player-name">
                                            {player.name}
                                        </div>
                                        <div className="leaderboard-page__player-meta">
                                            Matchday competitor
                                        </div>
                                    </div>
                                </div>

                                <div className="leaderboard-page__points">
                                    <span className="leaderboard-page__points-value">
                                        {player.points}
                                    </span>
                                    <span className="leaderboard-page__points-label">pts</span>
                                </div>

                                <div
                                    className={`leaderboard-page__change ${
                                        player.change.startsWith("+")
                                            ? "leaderboard-page__change--up"
                                            : player.change.startsWith("-")
                                                ? "leaderboard-page__change--down"
                                                : "leaderboard-page__change--neutral"
                                    }`}
                                >
                                    {player.change}
                                </div>
                            </article>
                        ))}
                    </div>
                </section>
            </main>
        </div>
    );
}
