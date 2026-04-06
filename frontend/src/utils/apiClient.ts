import { TOKEN_KEY } from "./constants";

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function apiClient(endpoint: string, options: RequestInit = {}) {
    const token = localStorage.getItem(TOKEN_KEY);

    const response = await fetch(`${BASE_URL}${endpoint}`, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
            ...options.headers,
        },
    });

    if (!response.ok) {
        const data = await response.json().catch(() => null);
        throw new Error(data?.message ?? "Something went wrong.");
    }

    return response.json();
}
