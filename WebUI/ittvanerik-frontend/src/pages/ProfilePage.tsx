import "./ProfilePage.css";
import {Link, useNavigate} from "react-router-dom";
import {useAuth} from "../context/AuthContext.tsx";

type ProfileStatProps = {
    label: string;
    value: string;
};

function ProfileStat({ label, value }: ProfileStatProps) {
    return (
        <div className="profile-page__stat">
            <span className="profile-page__stat-value">{value}</span>
            <span className="profile-page__stat-label">{label}</span>
        </div>
    );
}

type ProfileInfoItemProps = {
    label: string;
    value: string;
};

function ProfileInfoItem({ label, value }: ProfileInfoItemProps) {
    return (
        <div className="profile-page__info-item">
            <span className="profile-page__info-label">{label}</span>
            <span className="profile-page__info-value">{value}</span>
        </div>
    );
}

export default function ProfilePage() {

    const { logout, isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login", { replace: true });
    };
    
    return (
        <div className="profile-page">
            <div className="profile-page__bg-circle profile-page__bg-circle--top" />
            <div className="profile-page__bg-circle profile-page__bg-circle--bottom" />

            <header className="profile-page__header">
                <Link to="/">
                    <div className="profile-page__brand">
                        <div className="profile-page__brand-icon">⚽</div>
                        <div>
                            <div className="profile-page__brand-label">Football Prediction</div>
                            <div className="profile-page__brand-name">TipZone</div>
                        </div>
                    </div>
                </Link>

                <nav className="profile-page__nav">
                    <Link to="/" className="profile-page__nav-link">Home</Link>
                    <Link to="/matches" className="profile-page__nav-link">Matches</Link>
                    <Link to="/my-tips" className="profile-page__nav-link">My Tips</Link>
                    <Link to="/groups" className="profile-page__nav-link">My Groups</Link>
                    <Link
                        to="/profile"
                        className="profile-page__nav-link profile-page__nav-link--active"
                    >
                        Profile
                    </Link>
                    {isAuthenticated ? (
                        <button
                            type="button"
                            onClick={handleLogout}
                            className="profile-page__nav-button"
                        >
                            Log out
                        </button>
                    ) : (
                        <Link to="/login" className="profile-page__nav-button">Login</Link>
                    )}
                </nav>
            </header>

            <main className="profile-page__content">
                <section className="profile-page__hero">
                    <div className="profile-page__card profile-page__card--intro">
                        <div className="profile-page__avatar">R</div>

                        <div className="profile-page__intro-text">
                            <div className="profile-page__eyebrow">My Profile</div>
                            <h1 className="profile-page__title">Rdiesarkozi</h1>
                            <p className="profile-page__description">
                                Manage account details, review prediction activity, and keep track of performance in one place.
                            </p>
                        </div>
                    </div>

                    <div className="profile-page__card profile-page__card--stats">
                        <div className="profile-page__badge">Season Overview</div>

                        <div className="profile-page__stats">
                            <ProfileStat label="Predictions" value="48" />
                            <ProfileStat label="Points" value="126" />
                            <ProfileStat label="Rank" value="\#12" />
                        </div>
                    </div>
                </section>

                <section className="profile-page__grid">
                    <div className="profile-page__card">
                        <h2 className="profile-page__section-title">Account Details</h2>

                        <div className="profile-page__info-list">
                            <ProfileInfoItem label="Username" value="rdiesarkozi" />
                            <ProfileInfoItem label="Email" value="rdiesarkozi@example.com" />
                            <ProfileInfoItem label="Member Since" value="January 2025" />
                            <ProfileInfoItem label="Favorite Team" value="Not set" />
                        </div>
                    </div>

                    <div className="profile-page__card">
                        <h2 className="profile-page__section-title">Recent Activity</h2>

                        <div className="profile-page__activity-list">
                            <div className="profile-page__activity-item">
                                <span className="profile-page__activity-title">Submitted prediction for Matchday 12</span>
                                <span className="profile-page__activity-time">2 hours ago</span>
                            </div>

                            <div className="profile-page__activity-item">
                                <span className="profile-page__activity-title">Earned 9 points this week</span>
                                <span className="profile-page__activity-time">Yesterday</span>
                            </div>

                            <div className="profile-page__activity-item">
                                <span className="profile-page__activity-title">Joined the Weekend Fans group</span>
                                <span className="profile-page__activity-time">3 days ago</span>
                            </div>
                        </div>
                    </div>
                </section>
            </main>
        </div>
    );
}
