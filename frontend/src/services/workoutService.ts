import type { CreateWorkoutRequest } from "../types/workoutTypes";
import { apiClient } from "../utils/apiClient";

export const workoutService = {
  getForUser: async () => {
    return await apiClient("/workouts/me");
  },
  create: async (request: CreateWorkoutRequest) => {
    return await apiClient("/workouts", {
      method: "POST",
      body: JSON.stringify(request),
    });
  },
};
