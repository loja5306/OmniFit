import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";

export function useRemoveWorkoutExercise(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (workoutExerciseId: string) =>
      workoutService.removeExercise(workoutId, workoutExerciseId),
    scope: { id: `workout-${workoutId}` },
    onSuccess: () => {
      return queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
