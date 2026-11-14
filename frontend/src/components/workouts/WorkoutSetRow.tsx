import { X } from "lucide-react";
import type { WorkoutSetResponse } from "../../types/workoutTypes";
import { useRemoveWorkoutSet } from "../../hooks/useRemoveWorkoutSet";
import { useUpdateWorkoutSet } from "../../hooks/useUpdateWorkoutSet";
import { useState } from "react";

interface Props {
  workoutId: string;
  workoutExerciseId: string;
  workoutSet: WorkoutSetResponse;
  showDeleteButton: boolean;
  isReadOnly: boolean;
}

const WorkoutSetRow = ({
  workoutId,
  workoutExerciseId,
  workoutSet,
  showDeleteButton,
  isReadOnly,
}: Props) => {
  const [updatedSet, setUpdatedSet] = useState<WorkoutSetResponse>(workoutSet);

  const updateWorkoutSet = useUpdateWorkoutSet(workoutId);
  const removeWorkoutSet = useRemoveWorkoutSet(workoutId);

  const handleUpdateWorkoutSet = async () => {
    try {
      await updateWorkoutSet.mutateAsync({
        workoutExerciseId: workoutExerciseId,
        workoutSetId: updatedSet.id,
        request: {
          setNumber: updatedSet.setNumber,
          reps: updatedSet.reps,
          weight: updatedSet.weight,
        },
      });
    } catch (error) {
      console.error(error);
    }
  };

  const handleRemoveWorkoutSet = async () => {
    try {
      await removeWorkoutSet.mutateAsync({
        workoutExerciseId: workoutExerciseId,
        workoutSetId: workoutSet.id,
      });
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="p-3 rounded-md shadow-lg border border-gray-200">
      <div className="flex justify-between items-end gap-4">
        <p className="font-semibold flex-shrink-0 pb-1">
          Set {updatedSet.setNumber}
        </p>
        <div>
          <label>Reps</label>
          <input
            className="w-full px-2 py-1 text-sm border rounded-lg bg-white"
            type="number"
            disabled={isReadOnly}
            value={updatedSet.reps ?? ""}
            onChange={(e) =>
              setUpdatedSet({
                ...updatedSet,
                reps: Number.isNaN(e.target.valueAsNumber)
                  ? null
                  : e.target.valueAsNumber,
              })
            }
            onBlur={handleUpdateWorkoutSet}
          />
        </div>
        <div>
          <label>Weight</label>
          <input
            className="w-full px-2 py-1 text-sm border rounded-lg bg-white"
            type="number"
            disabled={isReadOnly}
            value={updatedSet.weight ?? ""}
            onChange={(e) =>
              setUpdatedSet({
                ...updatedSet,
                weight: Number.isNaN(e.target.valueAsNumber)
                  ? null
                  : e.target.valueAsNumber,
              })
            }
            onBlur={handleUpdateWorkoutSet}
          />
        </div>
        <div className="size-6 flex-shrink-0 mb-1">
          {showDeleteButton && !isReadOnly && (
            <button
              className="bg-red-400 size-full rounded-2xl font-bold text-xl cursor-pointer"
              onClick={handleRemoveWorkoutSet}
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
    </div>
  );
};

export default WorkoutSetRow;
