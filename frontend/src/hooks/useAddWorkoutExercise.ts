import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";
import type { WorkoutExerciseRequest } from "../types/workoutTypes";

export function useAddWorkoutExercise(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: WorkoutExerciseRequest) =>
      workoutService.addExercise(workoutId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
