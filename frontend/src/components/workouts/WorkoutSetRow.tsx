import { Check, X } from "lucide-react";
import type { WorkoutSetResponse } from "../../types/workoutTypes";
import { useRemoveWorkoutSet } from "../../hooks/useRemoveWorkoutSet";
import { useUpdateWorkoutSet } from "../../hooks/useUpdateWorkoutSet";
import { useEffect, useState } from "react";
import clsx from "clsx";

interface Props {
  workoutId: string;
  workoutExerciseId: string;
  workoutSet: WorkoutSetResponse;
  displaySetNumber: number;
  showDeleteButton: boolean;
  isReadOnly: boolean;
  onSavedChange: (workoutSetId: string, isSaved: boolean) => void;
}

interface Draft {
  reps?: number | null;
  weight?: number | null;
}

const WorkoutSetRow = ({
  workoutId,
  workoutExerciseId,
  workoutSet,
  displaySetNumber,
  showDeleteButton,
  isReadOnly,
  onSavedChange,
}: Props) => {
  const [draft, setDraft] = useState<Draft>({});

  const reps = draft.reps !== undefined ? draft.reps : workoutSet.reps;
  const weight = draft.weight !== undefined ? draft.weight : workoutSet.weight;
  const isSaved =
    draft.reps === undefined &&
    draft.weight === undefined &&
    reps !== null &&
    weight !== null;

  const updateWorkoutSet = useUpdateWorkoutSet(workoutId);
  const removeWorkoutSet = useRemoveWorkoutSet(workoutId);

  const repsInvalid = reps !== null && (reps < 0 || !Number.isInteger(reps));
  const weightInvalid = weight !== null && weight < 0;

  useEffect(() => {
    onSavedChange(workoutSet.id, isSaved);
    return () => onSavedChange(workoutSet.id, true);
  }, [workoutSet.id, isSaved, onSavedChange]);

  const handleUpdateWorkoutSet = async () => {
    try {
      if (repsInvalid || weightInvalid) return;
      if (draft.reps === undefined && draft.weight === undefined) return;

      await updateWorkoutSet.mutateAsync({
        workoutExerciseId: workoutExerciseId,
        workoutSetId: workoutSet.id,
        request: {
          setNumber: workoutSet.setNumber,
          reps: reps,
          weight: weight,
        },
      });

      setDraft((current) => {
        const next = { ...current };
        if (draft.reps !== undefined && current.reps === draft.reps)
          delete next.reps;
        if (draft.weight !== undefined && current.weight === draft.weight)
          delete next.weight;
        return next;
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
          Set {displaySetNumber}
        </p>
        <div>
          <label>Reps</label>
          <input
            className={`w-full px-2 py-1 text-sm border rounded-lg bg-white 
              ${repsInvalid ? "border-red-400" : ""}`}
            type="number"
            min={0}
            step={1}
            disabled={isReadOnly}
            value={reps ?? ""}
            onChange={(e) =>
              setDraft((d) => ({
                ...d,
                reps: Number.isNaN(e.target.valueAsNumber)
                  ? null
                  : e.target.valueAsNumber,
              }))
            }
          />
        </div>
        <div>
          <label>Weight</label>
          <input
            className={`w-full px-2 py-1 text-sm border rounded-lg bg-white
              ${weightInvalid ? "border-red-400" : ""}`}
            type="number"
            min={0}
            disabled={isReadOnly}
            value={weight ?? ""}
            onChange={(e) =>
              setDraft((d) => ({
                ...d,
                weight: Number.isNaN(e.target.valueAsNumber)
                  ? null
                  : e.target.valueAsNumber,
              }))
            }
          />
        </div>
        {!isReadOnly && (
          <label>
            <input
              type="checkbox"
              className="peer hidden"
              checked={isSaved}
              onChange={handleUpdateWorkoutSet}
              disabled={
                reps === null ||
                weight === null ||
                repsInvalid ||
                weightInvalid ||
                updateWorkoutSet.isPending
              }
            />
            <div
              className={clsx(
                "flex items-center justify-center rounded-full p-0.5",
                "border bg-white transition-colors cursor-pointer",
                "peer-hover:border-green-600",
                "peer-checked:bg-green-600 peer-checked:border-green-600",
                "[&>svg]:opacity-0 peer-checked:[&>svg]:opacity-100",
                "peer-disabled:bg-black/10 peer-disabled:border-black/30",
                "peer-disabled:cursor-default",
              )}
            >
              <Check className="text-white" />
            </div>
          </label>
        )}
        <div className="size-6 flex-shrink-0 mb-1">
          {showDeleteButton && !isReadOnly && (
            <button
              className="bg-red-400 size-full rounded-2xl font-bold text-xl cursor-pointer"
              disabled={removeWorkoutSet.isPending}
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
