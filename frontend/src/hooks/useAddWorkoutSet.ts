import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";
import type { WorkoutSetRequest } from "../types/workoutTypes";

export function useAddWorkoutSet(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workoutExerciseId,
      request,
    }: {
      workoutExerciseId: string;
      request: WorkoutSetRequest;
    }) => workoutService.addSet(workoutId, workoutExerciseId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
