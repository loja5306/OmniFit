import type { PagedResponse } from "../types/commonTypes";
import type {
  CreateWorkoutRequest,
  WorkoutQueryParameters,
  WorkoutResponse,
} from "../types/workoutTypes";
import { apiClient } from "../utils/apiClient";

export const workoutService = {
  getForUser: async (
    params?: WorkoutQueryParameters,
  ): Promise<PagedResponse<WorkoutResponse>> => {
    const searchParams = new URLSearchParams();

    if (params?.page != null) searchParams.set("page", String(params.page));
    if (params?.pageSize != null)
      searchParams.set("pageSize", String(params.pageSize));

    const qs = searchParams.toString();

    return await apiClient<PagedResponse<WorkoutResponse>>(
      `/workouts/me${qs ? `?${qs}` : ""}`,
    );
  },

  create: async (request: CreateWorkoutRequest): Promise<string> => {
    return await apiClient<string>("/workouts", {
      method: "POST",
      body: JSON.stringify(request),
    });
  },
};
