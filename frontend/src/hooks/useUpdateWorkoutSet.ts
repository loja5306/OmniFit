import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";
import type { WorkoutSetRequest } from "../types/workoutTypes";

export function useUpdateWorkoutSet(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workoutExerciseId,
      workoutSetId,
      request,
    }: {
      workoutExerciseId: string;
      workoutSetId: string;
      request: WorkoutSetRequest;
    }) =>
      workoutService.updateSet(
        workoutId,
        workoutExerciseId,
        workoutSetId,
        request,
      ),
    onSuccess: () => {
      return queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
