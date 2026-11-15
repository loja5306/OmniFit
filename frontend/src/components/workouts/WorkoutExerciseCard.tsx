import { Plus, X } from "lucide-react";
import type { WorkoutExerciseResponse } from "../../types/workoutTypes";
import WorkoutSetRow from "./WorkoutSetRow";
import { useRemoveWorkoutExercise } from "../../hooks/useRemoveWorkoutExercise";
import { useAddWorkoutSet } from "../../hooks/useAddWorkoutSet";

interface Props {
  workoutId: string;
  workoutExercise: WorkoutExerciseResponse;
  isReadOnly: boolean;
}

const WorkoutExerciseCard = ({
  workoutId,
  workoutExercise,
  isReadOnly,
}: Props) => {
  const removeWorkoutExercise = useRemoveWorkoutExercise(workoutId);
  const addWorkoutSet = useAddWorkoutSet(workoutId);

  const handleRemoveWorkoutExercise = async () => {
    await removeWorkoutExercise.mutateAsync(workoutExercise.id);
  };

  const handleAddWorkoutSet = async () => {
    try {
      const nextSetNumber =
        Math.max(0, ...workoutExercise.workoutSets.map((ws) => ws.setNumber)) +
        1;

      await addWorkoutSet.mutateAsync({
        workoutExerciseId: workoutExercise.id,
        request: {
          setNumber: nextSetNumber,
          reps: null,
          weight: null,
        },
      });
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="p-3 rounded-md shadow-lg border border-gray-200">
      <div className="flex justify-between items-center">
        <p className="font-semibold text-lg mb-2">{workoutExercise.name}</p>
        <div className="size-6 mb-1">
          {!isReadOnly && (
            <button
              className="bg-red-400 size-full rounded-2xl font-bold text-xl cursor-pointer"
              disabled={removeWorkoutExercise.isPending}
              onClick={handleRemoveWorkoutExercise}
            >
              <X
                size={16}
                stroke="white"
                strokeWidth={3}
                className="font-bold m-auto"
              />
            </button>
          )}
        </div>
      </div>
      <div className="flex flex-col gap-2">
        {workoutExercise.workoutSets.map((workoutSet, i) => {
          return (
            <WorkoutSetRow
              key={workoutSet.id}
              workoutId={workoutId}
              workoutExerciseId={workoutExercise.id}
              workoutSet={workoutSet}
              displaySetNumber={i + 1}
              showDeleteButton={workoutExercise.workoutSets.length > 1}
              isReadOnly={isReadOnly}
            />
          );
        })}
      </div>
      {!isReadOnly && (
        <div className="w-full flex justify-center pt-4">
          <button
            onClick={handleAddWorkoutSet}
            disabled={addWorkoutSet.isPending}
            className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center gap-2 rounded-md cursor-pointer"
          >
            <Plus size={20} strokeWidth={3} />
            Add Set
          </button>
        </div>
      )}
    </div>
  );
};

export default WorkoutExerciseCard;
