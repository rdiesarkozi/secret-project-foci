import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { createGroup, getMyGroups, joinGroup, type GroupVisibility } from "../api/GroupApi";
import { useAuth } from "../context/AuthContext";
import "./GroupPage.css";
import NavigationMenu from "../components/NavigationMenu.tsx";

type Group = {
    id: string;
    name: string;
    visibility: string;
    joinCode: string;
    leagueId: number;
    seasonId: number;
    createdAtUtc: string;
};

export default function GroupPage() {
    const { token } = useAuth();
    const [groups, setGroups] = useState<Group[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [groupName, setGroupName] = useState("");
    const [groupVisibility, setGroupVisibility] = useState<GroupVisibility>("Public");
    const [seasonId, setSeasonId] = useState(2026);
    const [leagueId, setLeagueId] = useState(1);
    const [createLoading, setCreateLoading] = useState(false);
    const [createError, setCreateError] = useState("");
    const [createSuccess, setCreateSuccess] = useState("");

    const [joinCode, setJoinCode] = useState("");
    const [joinLoading, setJoinLoading] = useState(false);
    const [joinError, setJoinError] = useState("");
    const [joinSuccess, setJoinSuccess] = useState("");

    useEffect(() => {
        const loadGroups = async () => {
            try {
                setLoading(true);
                setError("");

                if (!token) {
                    throw new Error("You must be logged in to view groups.");
                }

                const data = await getMyGroups(token);
                setGroups(data);
            } catch (err) {
                setError(err instanceof Error ? err.message : "Something went wrong.");
            } finally {
                setLoading(false);
            }
        };

        void loadGroups();
    }, [token]);

    const handleCreateGroup = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            setCreateLoading(true);
            setCreateError("");
            setCreateSuccess("");

            if (!token) {
                throw new Error("You must be logged in to create a group.");
            }

            const trimmedName = groupName.trim();

            if (!trimmedName) {
                throw new Error("Group name is required.");
            }

            if (!Number.isFinite(leagueId) || leagueId <= 0) {
                throw new Error("League ID must be a positive number.");
            }

            if (!Number.isFinite(seasonId) || seasonId <= 0) {
                throw new Error("Season ID must be a positive number.");
            }

            const newGroup = await createGroup(token, trimmedName, leagueId, seasonId, groupVisibility);

            setGroups((prev) => {
                const exists = prev.some((group) => group.id === newGroup.id);
                return exists ? prev : [newGroup, ...prev];
            });

            setGroupName("");
            setGroupVisibility("Public");
            setSeasonId(2026);
            setLeagueId(1);
            setCreateSuccess("Group created successfully.");
        } catch (err) {
            setCreateError(err instanceof Error ? err.message : "Failed to create group.");
        } finally {
            setCreateLoading(false);
        }
    };

    const handleJoinGroup = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            setJoinLoading(true);
            setJoinError("");
            setJoinSuccess("");

            if (!token) {
                throw new Error("You must be logged in to join a group.");
            }

            const trimmedJoinCode = joinCode.trim();

            if (!trimmedJoinCode) {
                throw new Error("Join code is required.");
            }

            const joinedGroup = await joinGroup(token, trimmedJoinCode);

            setGroups((prev) => {
                const exists = prev.some((group) => group.id === joinedGroup.id);
                return exists ? prev : [joinedGroup, ...prev];
            });

            setJoinCode("");
            setJoinSuccess(`Joined "${joinedGroup.name}" successfully.`);
        } catch (err) {
            setJoinError(err instanceof Error ? err.message : "Failed to join group.");
        } finally {
            setJoinLoading(false);
        }
    };

    return (
        <div className="group-page">
            <div className="group-page__bg-circle group-page__bg-circle--top" />
            <div className="group-page__bg-circle group-page__bg-circle--bottom" />

            <header className="group-page__header">
                <div className="group-page__brand">
                    <div className="group-page__brand-icon">⚽</div>
                    <div>
                        <div className="group-page__brand-label">Football Prediction</div>
                        <div className="group-page__brand-name">TipZone</div>
                    </div>
                </div>

                <nav className="group-page__nav">
                    <NavigationMenu />
                </nav>
            </header>

            <main className="group-page__content">
                <section className="group-page__hero">
                    <div className="group-page__hero-copy">
                        <div className="group-page__eyebrow">Your Groups</div>
                        <h1 className="group-page__title">Manage and view your football groups</h1>
                        <p className="group-page__description">
                            See the groups you belong to, check join codes, and keep track of when each group was created.
                        </p>
                    </div>

                    <div className="group-page__summary-card">
                        <div className="group-page__summary-badge">Groups Overview</div>
                        <div className="group-page__summary-stat">
                            <div className="group-page__summary-value">{groups.length}</div>
                            <div className="group-page__summary-label">Total Groups</div>
                        </div>

                        <div className="group-page__summary-join">
                            <div className="group-page__summary-join-title">Join a group</div>
                            <form className="group-page__summary-form" onSubmit={handleJoinGroup}>
                                <div className="group-page__form-group">
                                    <input
                                        id="joinCode"
                                        type="text"
                                        className="group-page__input"
                                        value={joinCode}
                                        onChange={(e) => setJoinCode(e.target.value)}
                                        placeholder="Enter join code"
                                        maxLength={100}
                                    />
                                </div>

                                <button
                                    type="submit"
                                    className="group-page__create-button group-page__summary-button"
                                    disabled={joinLoading}
                                >
                                    {joinLoading ? "Joining..." : "Join Group"}
                                </button>
                            </form>

                            {joinError && (
                                <div className="group-page__state group-page__error group-page__summary-message">
                                    Error: {joinError}
                                </div>
                            )}

                            {joinSuccess && (
                                <div className="group-page__state group-page__success group-page__summary-message">
                                    {joinSuccess}
                                </div>
                            )}
                        </div>
                    </div>
                </section>

                <div className="group-page__forms-grid">
                <section className="group-page__create-card">
                    <div className="group-page__create-header">
                        <div className="group-page__eyebrow">Create Group</div>
                        <h2 className="group-page__create-title">Start a new football group</h2>
                    </div>

                    <form className="group-page__create-form" onSubmit={handleCreateGroup}>
                        <div className="group-page__form-group">
                            <label htmlFor="groupName" className="group-page__label">Group Name</label>
                            <input
                                id="groupName"
                                type="text"
                                className="group-page__input"
                                value={groupName}
                                onChange={(e) => setGroupName(e.target.value)}
                                placeholder="Enter group name"
                                maxLength={100}
                            />
                        </div>

                        <div className="group-page__form-group">
                            <label htmlFor="leagueId" className="group-page__label">League ID</label>
                            <input
                                id="leagueId"
                                type="number"
                                className="group-page__input"
                                value={leagueId}
                                onChange={(e) => setLeagueId(Number(e.target.value))}
                                min={1}
                            />
                        </div>

                        <div className="group-page__form-group">
                            <label htmlFor="seasonId" className="group-page__label">Season ID</label>
                            <input
                                id="seasonId"
                                type="number"
                                className="group-page__input"
                                value={seasonId}
                                onChange={(e) => setSeasonId(Number(e.target.value))}
                                min={1}
                            />
                        </div>

                        <div className="group-page__form-group">
                            <label htmlFor="groupVisibility" className="group-page__label">Visibility</label>
                            <select
                                id="groupVisibility"
                                className="group-page__select"
                                value={groupVisibility}
                                onChange={(e) => setGroupVisibility(e.target.value as GroupVisibility)}
                            >
                                <option value="Public">Public</option>
                                <option value="Private">Private</option>
                            </select>
                        </div>

                        <button
                            type="submit"
                            className="group-page__create-button"
                            disabled={createLoading}
                        >
                            {createLoading ? "Creating..." : "Create Group"}
                        </button>

                        {createError && (
                            <div className="group-page__state group-page__error">
                                Error: {createError}
                            </div>
                        )}

                        {createSuccess && (
                            <div className="group-page__state group-page__success">
                                {createSuccess}
                            </div>
                        )}
                    </form>
                </section>
                </div>

                {loading && <div className="group-page__state">Loading groups...</div>}
                {!loading && error && <div className="group-page__state group-page__error">Error: {error}</div>}

                {!loading && !error && (
                    <section className="group-page__list-wrap">
                        {groups.length === 0 ? (
                            <div className="group-page__state">No groups found.</div>
                        ) : (
                            <ul className="group-page__list">
                                {groups.map((group) => (
                                    <li key={group.id} className="group-page__item">
                                        <div className="group-page__item-content">
                                            <div className="group-page__item-main">
                                                <strong className="group-page__item-title">{group.name}</strong>
                                                <div className="group-page__item-meta">
                                                    <div>Visibility: {group.visibility}</div>
                                                    <div>Join code: {group.joinCode}</div>
                                                    <div>League ID: {group.leagueId}</div>
                                                    <div>Season ID: {group.seasonId}</div>
                                                    <div>
                                                        Created: {new Date(group.createdAtUtc).toLocaleString()}
                                                    </div>
                                                </div>
                                            </div>

                                            <Link
                                                to={`/leaderboard?groupId=${encodeURIComponent(group.id)}`}
                                                className="group-page__leaderboard-button"
                                            >
                                                View Leaderboard
                                            </Link>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </section>
                )}
            </main>
        </div>
    );
}
