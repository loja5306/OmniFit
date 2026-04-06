import { X } from "lucide-react";
import type { WorkoutSet } from "../../types/workoutTypes";

interface Props {
  set: WorkoutSet;
  exerciseIndex: number;
  showDeleteButton: boolean;
  onUpdateSet: (
    exerciseIndex: number,
    setNumber: number,
    field: "reps" | "weight",
    value: number,
  ) => void;
  onDeleteSet: (exerciseIndex: number, setNumber: number) => void;
}

const WorkoutSetRow = ({
  set,
  exerciseIndex,
  showDeleteButton,
  onUpdateSet,
  onDeleteSet,
}: Props) => {
  return (
    <div className="p-3 rounded-md shadow-lg border border-gray-200">
      <div className="flex justify-between items-end gap-4">
        <p className="font-semibold flex-shrink-0 pb-1">Set {set.setNumber}</p>
        <div>
          <label>Reps</label>
          <input
            className="w-full px-2 py-1 text-sm border rounded-lg bg-white"
            type="number"
            value={set.reps}
            onChange={(e) =>
              onUpdateSet(
                exerciseIndex,
                set.setNumber,
                "reps",
                e.target.valueAsNumber,
              )
            }
          />
        </div>
        <div>
          <label>Weight</label>
          <input
            className="w-full px-2 py-1 text-sm border rounded-lg bg-white"
            type="number"
            value={set.weight}
            onChange={(e) =>
              onUpdateSet(
                exerciseIndex,
                set.setNumber,
                "weight",
                e.target.valueAsNumber,
              )
            }
          />
        </div>
        <div className="size-6 flex-shrink-0 mb-1">
          {showDeleteButton && (
            <button
              className="bg-red-400 size-full rounded-2xl font-bold text-xl cursor-pointer"
              onClick={() => onDeleteSet(exerciseIndex, set.setNumber)}
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
