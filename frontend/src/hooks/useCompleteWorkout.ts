import { useMutation, useQueryClient } from "@tanstack/react-query";
import { workoutService } from "../services/workoutService";

export function useCompleteWorkout(workoutId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: () => workoutService.complete(workoutId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workouts"] });
    },
  });
}
