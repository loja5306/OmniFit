import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { CreateWorkoutRequest } from "../types/workoutTypes";
import { workoutService } from "../services/workoutService";

export function useCreateWorkout() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateWorkoutRequest) =>
      workoutService.create(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
