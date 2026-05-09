import type {
  CreateExerciseRequest,
  Exercise,
  ExerciseQueryParameters,
  UpdateExerciseRequest,
} from "../types/exerciseTypes";
import type { PagedResponse } from "../types/commonTypes";
import { apiClient } from "../utils/apiClient";

export const exerciseService = {
  getAll: async (
    params?: ExerciseQueryParameters,
  ): Promise<PagedResponse<Exercise>> => {
    const searchParams = new URLSearchParams();

    if (params?.page != null) searchParams.set("page", String(params.page));
    if (params?.pageSize != null)
      searchParams.set("pageSize", String(params.pageSize));

    const qs = searchParams.toString();

    return apiClient<PagedResponse<Exercise>>(
      `/exercises${qs ? `?${qs}` : ""}`,
    );
  },

  getById: async (id: string): Promise<Exercise> => {
    return apiClient<Exercise>(`/exercises/${id}`);
  },

  create: async (request: CreateExerciseRequest): Promise<string> => {
    return apiClient<string>("/exercises", {
      method: "POST",
      body: JSON.stringify(request),
    });
  },

  update: async (
    id: string,
    request: UpdateExerciseRequest,
  ): Promise<Exercise> => {
    return apiClient<Exercise>(`/exercises/${id}`, {
      method: "PUT",
      body: JSON.stringify(request),
    });
  },

  delete: async (id: string): Promise<void> => {
    return apiClient<void>(`/exercises/${id}`, { method: "DELETE" });
  },
};
