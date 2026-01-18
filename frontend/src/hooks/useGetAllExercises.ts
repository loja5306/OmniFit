import { useQuery } from "@tanstack/react-query";
import { exerciseService } from "../services/exerciseService";

export function useGetAllExercises() {
  return useQuery({
    queryKey: ["exercises"],
    queryFn: exerciseService.getAll,
  });
}
