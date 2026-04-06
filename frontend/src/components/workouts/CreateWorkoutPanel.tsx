import { useState } from "react";
import WorkoutExerciseCard from "./WorkoutExerciseCard";
import type {
  CreateWorkoutRequest,
  WorkoutForm,
} from "../../types/workoutTypes";
import SelectExerciseModal from "../exercises/SelectExerciseModal";
import type { Exercise } from "../../types/exerciseTypes";
import { Plus, Save, X } from "lucide-react";
import { useCreateWorkout } from "../../hooks/useCreateWorkout";

interface Props {
  onClose: () => void;
}

const CreateWorkoutPanel = ({ onClose }: Props) => {
  const [workout, setWorkout] = useState<WorkoutForm>({
    name: "",
    exercises: [],
  });
  const [isModalOpen, setIsModalOpen] = useState(false);

  const createWorkout = useCreateWorkout();

  const handleSubmit = async () => {
    const request: CreateWorkoutRequest = {
      name: workout.name,
      exercises: workout.exercises.map(({ exerciseId, sets }) => ({
        exerciseId,
        sets,
      })),
    };
    await createWorkout.mutateAsync(request);
    onClose();
  };

  const handleAddExercise = (exercise: Exercise) => {
    setWorkout((prev) => ({
      ...prev,
      exercises: [
        ...prev.exercises,
        {
          exerciseId: exercise.id,
          exerciseName: exercise.name,
          sets: [{ setNumber: 1, reps: 0, weight: 0 }],
        },
      ],
    }));
    setIsModalOpen(false);
  };

  const handleDeleteExercise = (exerciseIndex: number) => {
    setWorkout((prev) => ({
      ...prev,
      exercises: prev.exercises.filter((_, index) => index !== exerciseIndex),
    }));
  };

  const handleAddSet = (exerciseIndex: number) => {
    setWorkout((prev) => ({
      ...prev,
      exercises: prev.exercises.map((exercise, index) =>
        index === exerciseIndex
          ? {
              ...exercise,
              sets: [
                ...exercise.sets,
                {
                  setNumber: exercise.sets.length + 1,
                  weight: 0,
                  reps: 0,
                },
              ],
            }
          : exercise,
      ),
    }));
  };

  const handleUpdateSet = (
    exerciseIndex: number,
    setNumber: number,
    field: "reps" | "weight",
    value: number,
  ) => {
    setWorkout((prev) => ({
      ...prev,
      exercises: prev.exercises.map((exercise, index) =>
        index === exerciseIndex
          ? {
              ...exercise,
              sets: exercise.sets.map((set) =>
                set.setNumber === setNumber ? { ...set, [field]: value } : set,
              ),
            }
          : exercise,
      ),
    }));
  };

  const handleDeleteSet = (exerciseIndex: number, setNumber: number) => {
    setWorkout((prev) => ({
      ...prev,
      exercises: prev.exercises.map((exercise, index) =>
        index === exerciseIndex
          ? {
              ...exercise,
              sets: exercise.sets
                .filter((set) => set.setNumber !== setNumber)
                .map((set, index) => ({ ...set, setNumber: index + 1 })),
            }
          : exercise,
      ),
    }));
  };

  return (
    <div className="max-w-lg mx-auto bg-white mt-6 p-4 rounded-lg shadow-md">
      <div className="w-full flex justify-between items-center pb-4">
        <div className="flex gap-2">
          <label className="flex-shrink-0 font-semibold text-lg">
            Workout Name
          </label>
          <input
            type="text"
            value={workout.name}
            onChange={(e) => setWorkout({ ...workout, name: e.target.value })}
            className="w-full px-2 py-1 text-sm border rounded-lg bg-white"
          />
        </div>
        <button
          onClick={() => setIsModalOpen(true)}
          className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center gap-2 rounded-md cursor-pointer"
        >
          <Plus size={20} strokeWidth={3} />
          Add Exercise
        </button>
      </div>
      <div className="flex flex-col gap-2">
        {workout.exercises.map((exercise, index) => {
          return (
            <WorkoutExerciseCard
              key={index}
              exerciseIndex={index}
              workoutExercise={exercise}
              onAddSet={handleAddSet}
              onUpdateSet={handleUpdateSet}
              onDeleteSet={handleDeleteSet}
              onDeleteExercise={handleDeleteExercise}
            />
          );
        })}
      </div>
      <SelectExerciseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSelectExercise={(exercise) => handleAddExercise(exercise)}
      />
      <div className="w-full flex gap-2 pt-4">
        <button
          onClick={onClose}
          className="flex-1 bg-gray-200 text-gray-700 font-semibold py-1 px-2 flex items-center justify-center gap-2 rounded-md cursor-pointer"
        >
          <X size={20} strokeWidth={2} />
          Discard
        </button>
        <button
          onClick={handleSubmit}
          className="flex-1 bg-blue-400 text-white font-semibold py-1 px-2 flex items-center justify-center gap-2 rounded-md cursor-pointer"
        >
          <Save size={20} strokeWidth={2} />
          Save
        </button>
      </div>
    </div>
  );
};

export default CreateWorkoutPanel;
