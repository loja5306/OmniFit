export interface WorkoutSet {
  setNumber: number;
  reps: number;
  weight: number;
}

export interface WorkoutExercise {
  exerciseId: string;
  sets: WorkoutSet[];
}

export interface WorkoutForm {
  name: string;
  exercises: WorkoutExerciseForm[];
}

export interface WorkoutExerciseForm {
  exerciseId: string;
  exerciseName: string;
  sets: WorkoutSet[];
}

export interface CreateWorkoutRequest {
  name: string;
  exercises: WorkoutExercise[];
}

export interface WorkoutResponse {
  id: string;
  name: string;
  totalExercises: number;
}
