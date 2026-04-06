import { Plus, X } from "lucide-react";
import type { WorkoutExerciseForm } from "../../types/workoutTypes";
import WorkoutSetRow from "./WorkoutSetRow";

interface Props {
  exerciseIndex: number;
  workoutExercise: WorkoutExerciseForm;
  onAddSet: (exerciseIndex: number) => void;
  onUpdateSet: (
    exerciseIndex: number,
    setNumber: number,
    field: "reps" | "weight",
    value: number,
  ) => void;
  onDeleteSet: (exerciseIndex: number, setNumber: number) => void;
  onDeleteExercise: (exerciseIndex: number) => void;
}

const WorkoutExerciseCard = ({
  exerciseIndex,
  workoutExercise,
  onAddSet,
  onUpdateSet,
  onDeleteSet,
  onDeleteExercise,
}: Props) => {
  return (
    <div className="p-3 rounded-md shadow-lg border border-gray-200">
      <div className="flex justify-between items-center">
        <p className="font-semibold text-lg mb-2">
          {workoutExercise.exerciseName}
        </p>
        <div className="size-6 mb-1">
          <button
            className="bg-red-400 size-full rounded-2xl font-bold text-xl cursor-pointer"
            onClick={() => onDeleteExercise(exerciseIndex)}
          >
            <X
              size={16}
              stroke="white"
              strokeWidth={3}
              className="font-bold m-auto"
            />
          </button>
        </div>
      </div>
      <div className="flex flex-col gap-2">
        {workoutExercise.sets.map((set, index) => {
          return (
            <WorkoutSetRow
              key={index}
              set={set}
              exerciseIndex={exerciseIndex}
              showDeleteButton={workoutExercise.sets.length > 1}
              onUpdateSet={onUpdateSet}
              onDeleteSet={onDeleteSet}
            />
          );
        })}
      </div>
      <div className="w-full flex justify-center pt-4">
        <button
          onClick={() => onAddSet(exerciseIndex)}
          className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center gap-2 rounded-md cursor-pointer"
        >
          <Plus size={20} strokeWidth={3} />
          Add Set
        </button>
      </div>
    </div>
  );
};

export default WorkoutExerciseCard;
