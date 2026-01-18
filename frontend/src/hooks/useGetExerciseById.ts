import { useQuery } from "@tanstack/react-query";
import { exerciseService } from "../services/exerciseService";

export function useGetExerciseById(id: string) {
  return useQuery({
    queryKey: ["exercises", id],
    queryFn: () => exerciseService.getById(id),
  });
}
