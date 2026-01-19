import type { CreateExerciseRequest } from "../types/exerciseTypes";

const base_url = import.meta.env.VITE_API_BASE_URL;

export const exerciseService = {
  getAll: async () => {
    const response = await fetch(`${base_url}/exercises`);
    return await response.json();
  },
  getById: async (id: string) => {
    const response = await fetch(`${base_url}/exercises/${id}`);
    return await response.json();
  },
  create: async (request: CreateExerciseRequest) => {
    const response = await fetch(`${base_url}/exercises`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });
    if (!response.ok) {
      throw Error();
    }
    return await response.json();
  },
};
