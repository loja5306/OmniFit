export interface Exercise {
  id: string;
  name: string;
  description: string;
}

export interface CreateExerciseRequest {
  name: string;
  description?: string;
}

export interface UpdateExerciseRequest {
  name: string;
  description: string;
}

export interface ExerciseQueryParameters {
  page?: number;
  pageSize?: number;
}
