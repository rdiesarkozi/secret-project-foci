const API_BASE_URL = "http://localhost:5256/api";

export type LoginRequest = {
    username: string;
    password: string;
};

export type LoginResponse = {
    token: string;
};

export type RegisterRequest = {
    username: string;
    email: string;
    password: string;
};

export type RegisterResponse = {
    message?: string;
};

export async function login(payload: LoginRequest): Promise<LoginResponse> {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
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
                    : "Login failed";

        throw new Error(message);
    }

    return data as LoginResponse;
}

export async function register(payload: RegisterRequest): Promise<RegisterResponse> {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
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
                    : "Registration failed";

        throw new Error(message);
    }

    if (typeof data === "string") {
        return { message: data };
    }

    return (data as RegisterResponse) ?? {};
}
