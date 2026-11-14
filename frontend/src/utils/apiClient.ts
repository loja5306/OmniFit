import { TOKEN_KEY } from "./constants";

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export class ApiError extends Error {
  messages: string[];

  constructor(messages: string[]) {
    super(messages.join(","));
    this.messages = messages;
  }
}

export async function apiClient<T>(
  endpoint: string,
  options: RequestInit = {},
): Promise<T> {
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
    const errorMessages: string[] = data?.errors
      ? Object.values<string[]>(data.errors).flat()
      : ["Something went wrong"];
    throw new ApiError(errorMessages);
  }

  if (response.status === 204) return undefined as T;

  return response.json();
}
