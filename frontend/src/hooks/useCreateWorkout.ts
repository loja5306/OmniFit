import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { WorkoutRequest } from "../types/workoutTypes";
import { workoutService } from "../services/workoutService";

export function useCreateWorkout() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: WorkoutRequest) => workoutService.create(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
