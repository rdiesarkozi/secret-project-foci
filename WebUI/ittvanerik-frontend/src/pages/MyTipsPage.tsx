import { Link } from "react-router-dom";
import { useEffect, useMemo, useState } from "react";
import { getMyTips, type TipResponse } from "../api/tipApi";
import { useAuth } from "../context/AuthContext";
import "./MyTipsPage.css";

export default function MyTipsPage() {
    const { token } = useAuth();
    const [tips, setTips] = useState<TipResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadTips = async () => {
            if (!token) {
                setError("You must be logged in to view your tips.");
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                setError("");
                const data = await getMyTips(token);
                setTips(data);
            } catch (err: unknown) {
                const message = err instanceof Error ? err.message : "Failed to fetch tips";
                setError(message);
            } finally {
                setLoading(false);
            }
        };

        void loadTips();
    }, [token]);

    const summary = useMemo(() => {
        const open = tips.filter((tip) => tip.resultStatus === "Open").length;
        const closed = tips.filter((tip) => tip.resultStatus === "Closed").length;
        const points = tips.reduce((sum, tip) => sum + (tip.awardedPoints ?? 0), 0);

        return { open, closed, points };
    }, [tips]);

    return (
        <div className="my-tips-page">
            <div className="my-tips-page__bg-circle my-tips-page__bg-circle--top" />
            <div className="my-tips-page__bg-circle my-tips-page__bg-circle--bottom" />

            <header className="my-tips-page__header">
                <div className="my-tips-page__brand">
                    <div className="my-tips-page__brand-icon">⚽</div>
                    <div>
                        <div className="my-tips-page__brand-label">Football Prediction</div>
                        <div className="my-tips-page__brand-name">TipZone</div>
                    </div>
                </div>

                <nav className="my-tips-page__nav">
                    <Link to="/" className="my-tips-page__nav-link">Home</Link>
                    <Link to="/matches" className="my-tips-page__nav-link">Matches</Link>
                    <Link to="/leaderboard" className="my-tips-page__nav-link">Leaderboard</Link>
                    <Link to="/my-tips" className="my-tips-page__nav-link my-tips-page__nav-link--active">
                        My Tips
                    </Link>
                    <Link to="/profile" className="my-tips-page__nav-link">Profile</Link>
                </nav>
            </header>

            <main className="my-tips-page__content">
                <section className="my-tips-page__hero">
                    <div className="my-tips-page__hero-copy">
                        <div className="my-tips-page__eyebrow">Your Predictions</div>
                        <h1 className="my-tips-page__title">Track your submitted football tips</h1>
                        <p className="my-tips-page__description">
                            Review your predictions, check match status, and see awarded points.
                        </p>
                    </div>

                    <div className="my-tips-page__summary-card">
                        <div className="my-tips-page__summary-badge">Overview</div>
                        <div className="my-tips-page__summary-stat">
                            <span className="my-tips-page__summary-value">{tips.length}</span>
                            <span className="my-tips-page__summary-label">Total Tips</span>
                        </div>
                        <div className="my-tips-page__summary-stat">
                            <span className="my-tips-page__summary-value">{summary.open}</span>
                            <span className="my-tips-page__summary-label">Open</span>
                        </div>
                        <div className="my-tips-page__summary-stat">
                            <span className="my-tips-page__summary-value">{summary.points}</span>
                            <span className="my-tips-page__summary-label">Points Earned</span>
                        </div>
                    </div>
                </section>

                {loading && <div className="my-tips-page__state">Loading tips...</div>}
                {!loading && error && <div className="my-tips-page__error">{error}</div>}

                {!loading && !error && tips.length === 0 && (
                    <div className="my-tips-page__state">No tips found yet.</div>
                )}

                {!loading && !error && tips.length > 0 && (
                    <section className="my-tips-page__list-wrapper">
                        <section className="my-tips-page__list">
                            {tips.map((tip) => (
                                <article key={tip.id} className="my-tips-page__card">
                                    <div className="my-tips-page__card-top">
                                        <span className="my-tips-page__date">
                                            Submitted: {new Date(tip.submittedAtUtc).toLocaleString()}
                                        </span>
                                        <span
                                            className={`my-tips-page__status ${
                                                tip.resultStatus === "Closed"
                                                    ? "my-tips-page__status--closed"
                                                    : tip.resultStatus === "Pending"
                                                        ? "my-tips-page__status--pending"
                                                        : ""
                                            }`}
                                        >
                                            {tip.resultStatus}
                                        </span>
                                    </div>
    
                                    <div className="my-tips-page__teams">
                                        <div className="my-tips-page__team my-tips-page__team--home">
                                            {tip.homeTeamName}
                                        </div>
                                        <div className="my-tips-page__prediction">
                                            {tip.predictedHomeScore} - {tip.predictedAwayScore}
                                        </div>
                                        <div className="my-tips-page__team my-tips-page__team--away">
                                            {tip.awayTeamName}
                                        </div>
                                    </div>
    
                                    <div className="my-tips-page__meta">
                                        <span>
                                            Actual:{" "}
                                            {tip.actualHomeScore !== null && tip.actualAwayScore !== null
                                                ? `${tip.actualHomeScore} - ${tip.actualAwayScore}`
                                                : "Not available"}
                                        </span>
                                        <span>Points: {tip.awardedPoints ?? 0}</span>
                                    </div>
                                </article>
                            ))}
                        </section>
                    </section>
                )}
            </main>
        </div>
    );
}
