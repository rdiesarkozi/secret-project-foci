import {Link, useSearchParams} from "react-router-dom";
import { useEffect, useMemo, useState } from "react";
import { getOverallLeaderboard, type LeaderboardEntry } from "../api/leaderboardApi";
import "./LeaderboardPage.css";

export default function LeaderboardPage() {
    const [searchParams] = useSearchParams();
    const groupId = searchParams.get("groupId") ?? "";

    const [leaderboard, setLeaderboard] = useState<LeaderboardEntry[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadLeaderboard = async () => {
            try {
                setLoading(true);
                setError("");
                const data = await getOverallLeaderboard(groupId);
                setLeaderboard(data);
            } catch (err: unknown) {
                const message = err instanceof Error ? err.message : "Failed to fetch leaderboard";
                setError(message);
            } finally {
                setLoading(false);
            }
        };

        void loadLeaderboard();
    }, []);

    const topScore = useMemo(
        () => (leaderboard.length > 0 ? Math.max(...leaderboard.map((player) => player.points)) : 0),
        [leaderboard]
    );

    return (
        <div className="leaderboard-page">
            <div className="leaderboard-page__bg-circle leaderboard-page__bg-circle--top" />
            <div className="leaderboard-page__bg-circle leaderboard-page__bg-circle--bottom" />

            <header className="leaderboard-page__header">
                <Link to="/">
                    <div className="home-page__brand">
                        <div className="home-page__brand-icon">⚽</div>
                        <div className="home-page__brand-text">
                            <div className="home-page__brand-label">Football Prediction</div>
                            <div className="home-page__brand-name">TipZone</div>
                        </div>
                    </div>
                </Link>

                <nav className="leaderboard-page__nav">
                    <Link to="/" className="leaderboard-page__nav-link">Home</Link>
                    <Link to="/matches" className="leaderboard-page__nav-link">Matches</Link>
                    <Link
                        to="/leaderboard"
                        className="leaderboard-page__nav-link leaderboard-page__nav-link--active"
                    >
                        Leaderboard
                    </Link>
                    <Link to="/my-tips" className="leaderboard-page__nav-link">My Tips</Link>
                    <Link to="/profile" className="leaderboard-page__nav-link">Profile</Link>
                    <Link to="/groups" className="group-page__nav-link">
                        My Groups
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
                            <span className="leaderboard-page__summary-value">{leaderboard.length}</span>
                            <span className="leaderboard-page__summary-label">Active Players</span>
                        </div>
                        <div className="leaderboard-page__summary-stat">
                            <span className="leaderboard-page__summary-value">{topScore}</span>
                            <span className="leaderboard-page__summary-label">Top Score</span>
                        </div>
                    </div>
                </section>

                {loading && <div>Loading leaderboard...</div>}
                {!loading && error && <div>{error}</div>}

                {!loading && !error && (
                    <section className="leaderboard-page__table-wrap">
                        <div className="leaderboard-page__table-header">
                            <h2 className="leaderboard-page__table-title">Leaderboard standings</h2>
                        </div>

                        <div className="leaderboard-page__table">
                            {leaderboard.map((player) => (
                                <article className="leaderboard-page__row">
                                    <div className="leaderboard-page__rank">{player.rank}</div>

                                    <div className="leaderboard-page__player">
                                        <div className="leaderboard-page__avatar">
                                            {player.username.charAt(0).toUpperCase()}
                                        </div>
                                        <div>
                                            <div className="leaderboard-page__player-name">
                                                {player.username}
                                            </div>
                                            <div className="leaderboard-page__player-meta">
                                                Group competitor
                                            </div>
                                        </div>
                                    </div>

                                    <div className="leaderboard-page__points">
                                        <span className="leaderboard-page__points-value">
                                            {player.points}
                                        </span>
                                        <span className="leaderboard-page__points-label">pts</span>
                                    </div>

                                    <div className="leaderboard-page__change leaderboard-page__change--neutral">
                                        Rank {player.rank}
                                    </div>
                                </article>
                            ))}
                        </div>
                    </section>
                )}
            </main>
        </div>
    );
}
