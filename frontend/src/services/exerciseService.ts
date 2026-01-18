import type { CreateExerciseRequest } from "../types/exerciseTypes";

export const exerciseService = {
  getAll: async () => {
    const response = await fetch("https://localhost:7223/exercises");
    return await response.json();
  },
  getById: async (id: string) => {
    const response = await fetch(`https://localhost:7223/exercises/${id}`);
    return await response.json();
  },
  create: async (request: CreateExerciseRequest) => {
    const response = await fetch("https://localhost:7223/exercises", {
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
