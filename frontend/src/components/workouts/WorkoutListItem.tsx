import type { WorkoutResponse } from "../../types/workoutTypes";

interface Props {
  workout: WorkoutResponse;
}

const WorkoutListItem = ({ workout }: Props) => {
  return (
    <div>
      <p className="font-semibold">{workout.name}</p>
      <p className="text-sm text-gray-500">
        {workout.totalExercises}{" "}
        {workout.totalExercises === 1 ? "exercise" : "exercises"}
      </p>
    </div>
  );
};

export default WorkoutListItem;
