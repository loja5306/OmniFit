import { useQuery } from "@tanstack/react-query";
import { exerciseService } from "../services/exerciseService";
import type { ExerciseQueryParameters } from "../types/exerciseTypes";

export function useGetAllExercises(params?: ExerciseQueryParameters) {
  return useQuery({
    queryKey: ["exercises", params],
    queryFn: () => exerciseService.getAll(params),
  });
}
