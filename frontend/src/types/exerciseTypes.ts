export interface Exercise {
  id: string;
  name: string;
  description?: string;
}

export interface CreateExerciseRequest {
  name: string;
  description?: string;
}
