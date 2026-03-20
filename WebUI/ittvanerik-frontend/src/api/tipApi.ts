export type TipResultStatus = "Open" | "Closed" | "Pending";

export type TipResponse = {
    id: string;
    userId: string;
    user: unknown | null;
    matchId: number;
    leagueId: number;
    seasonId: number;
    predictedHomeScore: number;
    predictedAwayScore: number;
    homeTeamName: string;
    awayTeamName: string;
    actualHomeScore: number | null;
    actualAwayScore: number | null;
    submittedAtUtc: string;
    lockedAtUtc: string;
    resultStatus: TipResultStatus;
    awardedPoints: number | null;
};

const API_BASE_URL = "http://localhost:5256/api";

export async function getMyTips(token: string): Promise<TipResponse[]> {
    const response = await fetch(`${API_BASE_URL}/Tip/get`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
    });

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
                    : "Failed to fetch tips";

        throw new Error(message);
    }

    return data as TipResponse[];
}
