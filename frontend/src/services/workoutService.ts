import type { PagedResponse } from "../types/commonTypes";
import type {
  WorkoutRequest,
  WorkoutQueryParameters,
  WorkoutSummaryResponse,
  WorkoutExerciseRequest,
  WorkoutSetRequest,
  WorkoutSetResponse,
  WorkoutResponse,
} from "../types/workoutTypes";
import { apiClient } from "../utils/apiClient";

export const workoutService = {
  getById: async (workoutId: string): Promise<WorkoutResponse> => {
    return await apiClient<WorkoutResponse>(`/workouts/${workoutId}`);
  },

  getForUser: async (
    params?: WorkoutQueryParameters,
  ): Promise<PagedResponse<WorkoutSummaryResponse>> => {
    const searchParams = new URLSearchParams();

    if (params?.page != null) searchParams.set("page", String(params.page));
    if (params?.pageSize != null)
      searchParams.set("pageSize", String(params.pageSize));

    const qs = searchParams.toString();

    return await apiClient<PagedResponse<WorkoutSummaryResponse>>(
      `/workouts/me${qs ? `?${qs}` : ""}`,
    );
  },

  create: async (request: WorkoutRequest): Promise<string> => {
    return await apiClient<string>("/workouts", {
      method: "POST",
      body: JSON.stringify(request),
    });
  },

  addExercise: async (
    workoutId: string,
    request: WorkoutExerciseRequest,
  ): Promise<string> => {
    return await apiClient<string>(`/workouts/${workoutId}/exercises`, {
      method: "POST",
      body: JSON.stringify(request),
    });
  },

  removeExercise: async (
    workoutId: string,
    workoutExerciseId: string,
  ): Promise<void> => {
    return await apiClient<void>(
      `/workouts/${workoutId}/exercises/${workoutExerciseId}`,
      {
        method: "DELETE",
      },
    );
  },

  addSet: async (
    workoutId: string,
    workoutExerciseId: string,
    request: WorkoutSetRequest,
  ): Promise<string> => {
    return await apiClient<string>(
      `/workouts/${workoutId}/exercises/${workoutExerciseId}/sets`,
      {
        method: "POST",
        body: JSON.stringify(request),
      },
    );
  },

  updateSet: async (
    workoutId: string,
    workoutExerciseId: string,
    workoutSetId: string,
    request: WorkoutSetRequest,
  ): Promise<WorkoutSetResponse> => {
    return await apiClient<WorkoutSetResponse>(
      `/workouts/${workoutId}/exercises/${workoutExerciseId}/sets/${workoutSetId}`,
      {
        method: "PUT",
        body: JSON.stringify(request),
      },
    );
  },

  removeSet: async (
    workoutId: string,
    workoutExerciseId: string,
    workoutSetId: string,
  ): Promise<void> => {
    return await apiClient<void>(
      `/workouts/${workoutId}/exercises/${workoutExerciseId}/sets/${workoutSetId}`,
      {
        method: "DELETE",
      },
    );
  },

  complete: async (workoutId: string): Promise<void> => {
    return await apiClient<void>(`/workouts/${workoutId}/complete`, {
      method: "POST",
    });
  },
};
