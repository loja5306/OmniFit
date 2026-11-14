import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";

export function useRemoveWorkoutSet(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workoutExerciseId,
      workoutSetId,
    }: {
      workoutExerciseId: string;
      workoutSetId: string;
    }) => workoutService.removeSet(workoutId, workoutExerciseId, workoutSetId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
