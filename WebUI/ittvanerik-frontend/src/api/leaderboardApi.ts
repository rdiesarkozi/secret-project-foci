const API_BASE_URL = "http://localhost:5256/api";

export type LeaderboardEntry = {
    username: string;
    points: number;
    rank: number;
};

export async function getOverallLeaderboard(groupId: string): Promise<LeaderboardEntry[]> {
    const response = await fetch(
        `${API_BASE_URL}/Leaderboard/overall?groupId=${encodeURIComponent(groupId)}`,
        {
            headers: {
                "Content-Type": "application/json",
            },
        }
    );

    const contentType = response.headers.get("content-type") ?? "";
    const data = contentType.includes("application/json")
        ? await response.json()
        : await response.text();

    if (!response.ok) {
        const message =
            typeof data === "string"
                ? data
                : typeof data === "object" && data && "message" in data
                    ? String(data.message)
                    : "Failed to fetch leaderboard";

        throw new Error(message);
    }

    return data as LeaderboardEntry[];
}
