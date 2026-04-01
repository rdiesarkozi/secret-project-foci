import { Link } from "react-router-dom";
import { useEffect, useMemo, useState } from "react";
import {
    getMyGroups,
    getGroupById,
    type GroupResponse,
} from "../api/GroupApi";
import {
    getAllUpcomingMatchesByLeague,
    type MatchResponse,
} from "../api/matchesApi";
import { createTip, getMyTips, updateTip } from "../api/tipApi";
import GroupSelect from "../components/GroupSelect";
import "./MatchesPage.css";

type PredictionState = {
    homeScoreTip: string;
    awayScoreTip: string;
    isSaving: boolean;
    hasExistingTip: boolean;
};

type PredictionMap = Record<number, PredictionState>;

const emptyPrediction: PredictionState = {
    homeScoreTip: "",
    awayScoreTip: "",
    isSaving: false,
    hasExistingTip: false,
};

export default function MatchesPage() {
    const token = localStorage.getItem("token") ?? "";

    const [groups, setGroups] = useState<GroupResponse[]>([]);
    const [selectedGroupId, setSelectedGroupId] = useState("");
    const [selectedGroup, setSelectedGroup] = useState<GroupResponse | null>(null);

    const [matches, setMatches] = useState<MatchResponse[]>([]);
    const [predictions, setPredictions] = useState<PredictionMap>({});

    const [groupsLoading, setGroupsLoading] = useState(true);
    const [matchesLoading, setMatchesLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;

        async function loadGroups() {
            try {
                setGroupsLoading(true);
                setError(null);

                const data = await getMyGroups(token);

                if (cancelled) {
                    return;
                }

                setGroups(data);

                if (data.length > 0) {
                    setSelectedGroupId(String(data[0].id));
                } else {
                    setSelectedGroupId("");
                    setSelectedGroup(null);
                    setMatches([]);
                    setPredictions({});
                }
            } catch (err) {
                if (!cancelled) {
                    setError(err instanceof Error ? err.message : "Failed to fetch groups");
                }
            } finally {
                if (!cancelled) {
                    setGroupsLoading(false);
                }
            }
        }

        if (!token) {
            setGroupsLoading(false);
            setError("Missing authentication token");
            return;
        }

        void loadGroups();

        return () => {
            cancelled = true;
        };
    }, [token]);

    useEffect(() => {
        let cancelled = false;

        async function loadGroupAndMatches() {
            if (!selectedGroupId || !token) {
                setSelectedGroup(null);
                setMatches([]);
                setPredictions({});
                return;
            }

            try {
                setMatchesLoading(true);
                setError(null);

                const group = await getGroupById(token, selectedGroupId);

                if (cancelled) {
                    return;
                }

                setSelectedGroup(group);

                const matchesData = await getAllUpcomingMatchesByLeague(
                    group.leagueId,
                    group.seasonId,
                    10
                );

                const tipsData = await getMyTips(token);

                if (cancelled) {
                    return;
                }

                setMatches(matchesData);

                const tipsByMatchId = new Map(
                    tipsData.map((tip) => [
                        tip.matchId,
                        {
                            homeScoreTip: String(tip.predictedHomeScore),
                            awayScoreTip: String(tip.predictedAwayScore),
                            isSaving: false,
                            hasExistingTip: true,
                        },
                    ])
                );

                const nextPredictions: PredictionMap = {};
                for (const match of matchesData) {
                    nextPredictions[match.fixtureId] =
                        tipsByMatchId.get(match.fixtureId) ?? { ...emptyPrediction };
                }

                setPredictions(nextPredictions);
            } catch (err) {
                if (!cancelled) {
                    setError(err instanceof Error ? err.message : "Failed to fetch matches");
                    setMatches([]);
                    setPredictions({});
                }
            } finally {
                if (!cancelled) {
                    setMatchesLoading(false);
                }
            }
        }

        void loadGroupAndMatches();

        return () => {
            cancelled = true;
        };
    }, [selectedGroupId, token]);

    const openMatchesCount = matches.length;

    const closingSoonCount = useMemo(() => {
        const now = Date.now();
        const twentyFourHours = 24 * 60 * 60 * 1000;

        return matches.filter((match) => {
            const kickoff = new Date(match.fixtureDate).getTime();
            return kickoff > now && kickoff - now <= twentyFourHours;
        }).length;
    }, [matches]);

    function formatDateTime(dateUtc: string) {
        const date = new Date(dateUtc);

        return {
            date: date.toLocaleDateString(),
            time: date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }),
        };
    }

    function hasMatchStarted(dateUtc: string) {
        return new Date(dateUtc).getTime() <= Date.now();
    }

    function getStatus(dateUtc: string) {
        if (hasMatchStarted(dateUtc)) {
            return "Closed";
        }

        const now = Date.now();
        const kickoff = new Date(dateUtc).getTime();
        const diff = kickoff - now;
        const twentyFourHours = 24 * 60 * 60 * 1000;

        if (diff <= twentyFourHours) {
            return "Closing Soon";
        }

        return "Open";
    }

    function updatePredictionValue(
        fixtureId: number,
        field: "homeScoreTip" | "awayScoreTip",
        value: string
    ) {
        setPredictions((prev) => ({
            ...prev,
            [fixtureId]: {
                ...(prev[fixtureId] ?? emptyPrediction),
                [field]: value,
            },
        }));
    }

    async function submitPrediction(match: MatchResponse) {
        const prediction = predictions[match.fixtureId] ?? emptyPrediction;

        if (
            !token ||
            !selectedGroup ||
            prediction.isSaving ||
            hasMatchStarted(match.fixtureDate)
        ) {
            return;
        }

        if (prediction.homeScoreTip === "" || prediction.awayScoreTip === "") {
            return;
        }

        setPredictions((prev) => ({
            ...prev,
            [match.fixtureId]: {
                ...(prev[match.fixtureId] ?? emptyPrediction),
                isSaving: true,
            },
        }));

        try {
            if (prediction.hasExistingTip) {
                await updateTip(token, {
                    fixtureId: match.fixtureId,
                    homeScoreTip: Number(prediction.homeScoreTip),
                    awayScoreTip: Number(prediction.awayScoreTip),
                });
            } else {
                await createTip(token, {
                    fixtureId: match.fixtureId,
                    leagueId: selectedGroup.leagueId,
                    seasonId: selectedGroup.seasonId,
                    homeScoreTip: Number(prediction.homeScoreTip),
                    awayScoreTip: Number(prediction.awayScoreTip),
                });
            }

            setPredictions((prev) => ({
                ...prev,
                [match.fixtureId]: {
                    ...(prev[match.fixtureId] ?? emptyPrediction),
                    isSaving: false,
                    hasExistingTip: true,
                },
            }));
        } catch (err) {
            console.error("Failed to save tip", err);
            setPredictions((prev) => ({
                ...prev,
                [match.fixtureId]: {
                    ...(prev[match.fixtureId] ?? emptyPrediction),
                    isSaving: false,
                },
            }));
        }
    }

    function handleKeyDown(
        event: React.KeyboardEvent<HTMLInputElement>,
        match: MatchResponse
    ) {
        if (event.key === "Enter") {
            event.preventDefault();
            void submitPrediction(match);
        }
    }

    return (
        <div className="matches-page">
            <div className="matches-page__bg-circle matches-page__bg-circle--top" />
            <div className="matches-page__bg-circle matches-page__bg-circle--bottom" />

            <header className="matches-page__header">
                <Link to="/">
                    <div className="home-page__brand">
                        <div className="home-page__brand-icon">⚽</div>
                        <div className="home-page__brand-text">
                            <div className="home-page__brand-label">Football Prediction</div>
                            <div className="home-page__brand-name">TipZone</div>
                        </div>
                    </div>
                </Link>

                <nav className="matches-page__nav">
                    <Link to="/" className="matches-page__nav-link">Home</Link>
                    <Link to="/matches" className="matches-page__nav-link matches-page__nav-link--active">
                        Matches
                    </Link>
                    <Link to="/profile" className="matches-page__nav-link">Profile</Link>
                    <Link to="/groups" className="matches-page__nav-link">My Groups</Link>
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

                        <div className="matches-page__filters">
                            <GroupSelect
                                groups={groups}
                                value={selectedGroupId}
                                onChange={setSelectedGroupId}
                                disabled={groupsLoading || matchesLoading}
                            />
                        </div>

                        {selectedGroup && (
                            <div className="matches-page__group-meta">
                                League ID: {selectedGroup.leagueId}  Season ID: {selectedGroup.seasonId}
                            </div>
                        )}
                    </div>

                    <div className="matches-page__summary-card">
                        <div className="matches-page__summary-badge">Matchday Overview</div>
                        <div className="matches-page__summary-stat">
                            <span className="matches-page__summary-value">{openMatchesCount}</span>
                            <span className="matches-page__summary-label">Open Matches</span>
                        </div>
                        <div className="matches-page__summary-stat">
                            <span className="matches-page__summary-value">{closingSoonCount}</span>
                            <span className="matches-page__summary-label">Closing Soon</span>
                        </div>
                    </div>
                </section>

                {groupsLoading || matchesLoading ? (
                    <section className="matches-page__list">
                        <div className="matches-page__state">Loading...</div>
                    </section>
                ) : error ? (
                    <section className="matches-page__list">
                        <div className="matches-page__state matches-page__error">{error}</div>
                    </section>
                ) : matches.length === 0 ? (
                    <section className="matches-page__list">
                        <div className="matches-page__state">No available matches found.</div>
                    </section>
                ) : (
                    <section className="matches-page__list">
                        {matches.map((match) => {
                            const formatted = formatDateTime(match.fixtureDate);
                            const status = getStatus(match.fixtureDate);
                            const prediction = predictions[match.fixtureId] ?? emptyPrediction;
                            const locked = hasMatchStarted(match.fixtureDate);

                            return (
                                <article key={match.fixtureId} className="matches-page__card">
                                    <div className="matches-page__card-top">
                                        <span className="matches-page__date">
                                            {formatted.date} {formatted.time}
                                        </span>
                                        <span
                                            className={`matches-page__status ${
                                                status === "Closing Soon"
                                                    ? "matches-page__status--warning"
                                                    : ""
                                            }`}
                                        >
                                            {status}
                                        </span>
                                    </div>

                                    <div className="matches-page__teams">
                                        <div className="matches-page__team__home">{match.homeTeam}</div>
                                        <div className="matches-page__vs">VS</div>
                                        <div className="matches-page__team__away">{match.awayTeam}</div>
                                    </div>

                                    <div className="matches-page__prediction">
                                        <div className="matches-page__prediction-score">
                                            <div className="matches-page__prediction-side">
                                                <span className="matches-page__prediction-label">Home</span>
                                                <input
                                                    className="matches-page__prediction-input"
                                                    type="number"
                                                    min="0"
                                                    value={prediction.homeScoreTip}
                                                    disabled={locked}
                                                    onChange={(e) =>
                                                        updatePredictionValue(
                                                            match.fixtureId,
                                                            "homeScoreTip",
                                                            e.target.value
                                                        )
                                                    }
                                                    onBlur={() => void submitPrediction(match)}
                                                    onKeyDown={(e) => handleKeyDown(e, match)}
                                                    placeholder="0"
                                                />
                                            </div>

                                            <span className="matches-page__prediction-separator">:</span>

                                            <div className="matches-page__prediction-side">
                                                <span className="matches-page__prediction-label">Away</span>
                                                <input
                                                    className="matches-page__prediction-input"
                                                    type="number"
                                                    min="0"
                                                    value={prediction.awayScoreTip}
                                                    disabled={locked}
                                                    onChange={(e) =>
                                                        updatePredictionValue(
                                                            match.fixtureId,
                                                            "awayScoreTip",
                                                            e.target.value
                                                        )
                                                    }
                                                    onBlur={() => void submitPrediction(match)}
                                                    onKeyDown={(e) => handleKeyDown(e, match)}
                                                    placeholder="0"
                                                />
                                            </div>
                                        </div>

                                        <div className="matches-page__prediction-meta">
                                            <span
                                                className={`matches-page__prediction-state ${
                                                    locked
                                                        ? "matches-page__prediction-state--locked"
                                                        : prediction.hasExistingTip
                                                            ? "matches-page__prediction-state--saved"
                                                            : ""
                                                }`}
                                            >
                                                {locked ? "Locked" : prediction.hasExistingTip ? "Saved" : "Not saved"}
                                            </span>
                                        </div>
                                    </div>
                                </article>
                            );
                        })}
                    </section>
                )}
            </main>
        </div>
    );
}
