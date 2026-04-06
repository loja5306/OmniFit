import type { LoginRequest, RegisterRequest } from "../types/authTypes";
import { apiClient } from "../utils/apiClient";

export const authService = {
  login: async (request: LoginRequest) => {
    try {
      return await apiClient("/auth/login", {
        method: "POST",
        body: JSON.stringify(request),
      });
    } catch {
      throw new Error("Invalid email or password.");
    }
  },
  register: async (request: RegisterRequest) => {
    try {
      return await apiClient("/auth/register", {
        method: "POST",
        body: JSON.stringify(request),
      });
    } catch {
      throw new Error("Registration failed. Email may already be in use.");
    }
  },
};
