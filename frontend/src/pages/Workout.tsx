import { useNavigate, useParams } from "react-router";
import { useGetWorkoutById } from "../hooks/useGetWorkoutById";
import WorkoutExerciseCard from "../components/workouts/WorkoutExerciseCard";
import SelectExerciseModal from "../components/exercises/SelectExerciseModal";
import { useState } from "react";
import { useAddWorkoutExercise } from "../hooks/useAddWorkoutExercise";
import type { Exercise } from "../types/exerciseTypes";
import { useCompleteWorkout } from "../hooks/useCompleteWorkout";
import ErrorBox from "../components/common/ErrorBox";
import { ApiError } from "../utils/apiClient";

const Workout = () => {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const { workoutId } = useParams();
  const navigate = useNavigate();

  const workout = useGetWorkoutById(workoutId!).data;
  const addWorkoutExercise = useAddWorkoutExercise(workoutId!);
  const completeWorkout = useCompleteWorkout(workoutId!);
  const isReadOnly = workout?.status === "Completed";

  const handleSelectExercise = async (exercise: Exercise) => {
    try {
      await addWorkoutExercise.mutateAsync({
        exerciseId: exercise.id,
      });
      setIsModalOpen(false);
    } catch (error) {
      console.error(error);
    }
  };

  const handleCompleteWorkout = async () => {
    try {
      await completeWorkout.mutateAsync();
      navigate("/workouts");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="max-w-lg mx-auto bg-white mt-6 p-4 rounded-lg shadow-md">
      <div className="flex justify-between items-center pb-4">
        <h2 className="text-xl font-semibold">{workout?.name}</h2>
        {!isReadOnly && (
          <button
            onClick={() => setIsModalOpen(true)}
            className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center rounded-md cursor-pointer"
          >
            Add Exercise
          </button>
        )}
      </div>
      {completeWorkout.isError && (
        <ErrorBox
          errorMessages={
            completeWorkout.error instanceof ApiError
              ? completeWorkout.error.messages
              : [completeWorkout.error.message]
          }
        />
      )}
      {workout?.workoutExercises.map((workoutExercise) => (
        <WorkoutExerciseCard
          key={workoutExercise.id}
          workoutId={workout.id}
          workoutExercise={workoutExercise}
          isReadOnly={isReadOnly}
        />
      ))}
      {!isReadOnly && (
        <button
          onClick={handleCompleteWorkout}
          disabled={completeWorkout.isPending}
          className="bg-green-500 text-white font-semibold py-1 px-2 mt-4 mx-auto flex items-center rounded-md cursor-pointer"
        >
          Complete Workout
        </button>
      )}
      <SelectExerciseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSelectExercise={(exercise: Exercise) =>
          handleSelectExercise(exercise)
        }
      />
    </div>
  );
};

export default Workout;
