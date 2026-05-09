import { useQuery } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";
import type { WorkoutQueryParameters } from "../types/workoutTypes";

export function useGetWorkoutsForUser(params?: WorkoutQueryParameters) {
  return useQuery({
    queryKey: ["workouts", params],
    queryFn: () => workoutService.getForUser(params),
  });
}
