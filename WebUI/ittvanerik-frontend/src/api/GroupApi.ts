const API_BASE_URL = "http://localhost:5256/api";

export type GroupVisibility = "Private" | "Public";

export type GroupResponse = {
    id: string;
    name: string;
    joinCode: string;
    visibility: GroupVisibility;
    leagueId: number;
    seasonId: number;
    createdAtUtc: string;
};

function mapVisibilityToApiValue(visibility: GroupVisibility): number {
    return visibility === "Public" ? 1 : 0;
}

function mapApiGroup(raw: unknown): GroupResponse {
    const group = raw as Record<string, unknown>;

    return {
        id: String(group.id ?? group.groupId ?? ""),
        name: String(group.name ?? group.groupName ?? ""),
        joinCode: String(group.joinCode ?? ""),
        visibility:
            Number(group.visibility) === 1 || group.visibility === "Public"
                ? "Public"
                : "Private",
        leagueId: Number(group.leagueId ?? group.leagueID ?? group.LeagueId ?? 0),
        seasonId: Number(group.seasonId ?? group.seasonID ?? group.SeasonId ?? 0),
        createdAtUtc: String(group.createdAtUtc ?? group.createdAt ?? ""),
    };
}

export async function getMyGroups(token: string): Promise<GroupResponse[]> {
    const response = await fetch(`${API_BASE_URL}/Group/my-groups`, {
        method: "GET",
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
                    : "Failed to fetch groups";

        throw new Error(message);
    }

    return Array.isArray(data) ? data.map(mapApiGroup) : [];
}

export async function createGroup(
    token: string,
    groupName: string,
    leagueId: number,
    seasonId: number,
    visibility: GroupVisibility
): Promise<GroupResponse> {
    const response = await fetch(`${API_BASE_URL}/Group/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
            groupName,
            visibility: mapVisibilityToApiValue(visibility),
            leagueId,
            seasonId,
        }),
    });

    const contentType = response.headers.get("content-type") ?? "";
    const data = contentType.includes("application/json")
        ? await response.json()
        : await response.text();

    if (!response.ok) {
        const message =
            typeof data === "string"
                ? data
                : typeof data === "object" && data && "errors" in data
                    ? JSON.stringify(data)
                    : typeof data === "object" && data && "message" in data
                        ? String(data.message)
                        : "Failed to create group";

        throw new Error(message);
    }

    return mapApiGroup(data);
}

export async function getGroupById(
    token: string,
    groupId: string
): Promise<GroupResponse> {
    const response = await fetch(`${API_BASE_URL}/Group/${groupId}`, {
        method: "GET",
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
                    : "Failed to fetch group details";

        throw new Error(message);
    }

    return mapApiGroup(data);
}
