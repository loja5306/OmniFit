import { useQuery } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";

export function useGetWorkoutById(workoutId: string) {
  return useQuery({
    queryKey: ["workouts", workoutId],
    queryFn: () => workoutService.getById(workoutId),
  });
}
