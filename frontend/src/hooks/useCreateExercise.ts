import { useMutation, useQueryClient } from "@tanstack/react-query";
import { exerciseService } from "../services/exerciseService";
import type { CreateExerciseRequest } from "../types/exerciseTypes";

export function useCreateExercise() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateExerciseRequest) =>
      exerciseService.create(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["exercises"] });
    },
  });
}
