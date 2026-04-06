import { useQuery } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";

export function useGetWorkoutsForUser() {
  return useQuery({
    queryKey: ["workouts"],
    queryFn: workoutService.getForUser,
  });
}
