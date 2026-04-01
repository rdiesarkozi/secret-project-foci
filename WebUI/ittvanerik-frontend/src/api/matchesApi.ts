// src/api/matchesApi.ts
const API_BASE_URL = "http://localhost:5256/api";

export type MatchResponse = {
    fixtureId: number;
    fixtureDate: string;
    homeTeam: string;
    awayTeam: string;
};

function mapApiMatch(raw: unknown): MatchResponse {
    const match = raw as Record<string, unknown>;

    return {
        fixtureId: Number(match.fixtureId ?? match.fixtureID ?? match.id ?? 0),
        fixtureDate: String(match.fixtureDate ?? match.date ?? ""),
        homeTeam: String(match.homeTeam ?? match.homeTeamName ?? ""),
        awayTeam: String(match.awayTeam ?? match.awayTeamName ?? ""),
    };
}

export async function getAllUpcomingMatchesByLeague(
    leagueId: number,
    seasonId: number,
    next = 10
): Promise<MatchResponse[]> {
    const response = await fetch(
        `${API_BASE_URL}/Fixture/upcoming?league=${leagueId}&season=${seasonId}&numberOfNextMatches=${next}`,
        {
            method: "GET",
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
                : "Failed to fetch upcoming matches";

        throw new Error(message);
    }

    return Array.isArray(data) ? data.map(mapApiMatch) : [];
}
