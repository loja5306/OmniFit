import type { CreateExerciseRequest } from "../types/exerciseTypes";
import { apiClient } from "../utils/apiClient";

export const exerciseService = {
  getAll: async () => {
    return await apiClient("/exercises");
  },
  getById: async (id: string) => {
    return await apiClient(`/exercises/${id}`);
  },
  create: async (request: CreateExerciseRequest) => {
    return await apiClient("/exercises", {
      method: "POST",
      body: JSON.stringify(request),
    });
  },
};
