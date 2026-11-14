export interface WorkoutRequest {
  name: string;
}

export interface WorkoutExerciseRequest {
  exerciseId: string;
}

export interface WorkoutSetRequest {
  setNumber: number;
  reps: number | null;
  weight: number | null;
}

export interface WorkoutQueryParameters {
  page?: number;
  pageSize?: number;
}

export interface WorkoutSummaryResponse {
  id: string;
  name: string;
  status: "InProgress" | "Completed";
  completedOn: string | null;
  totalExercises: number;
}

export interface WorkoutResponse {
  id: string;
  name: string;
  status: "InProgress" | "Completed";
  completedOn: string | null;
  workoutExercises: WorkoutExerciseResponse[];
}

export interface WorkoutExerciseResponse {
  id: string;
  exerciseId: string;
  name: string;
  workoutSets: WorkoutSetResponse[];
}

export interface WorkoutSetResponse {
  id: string;
  workoutExerciseId: string;
  setNumber: number;
  reps: number | null;
  weight: number | null;
}
