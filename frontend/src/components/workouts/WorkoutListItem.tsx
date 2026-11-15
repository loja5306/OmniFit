import { Link } from "react-router";
import type { WorkoutSummaryResponse } from "../../types/workoutTypes";
import StatusBadge from "./StatusBadge";

interface Props {
  workout: WorkoutSummaryResponse;
}

const WorkoutListItem = ({ workout }: Props) => {
  return (
    <div className="flex justify-between">
      <div>
        <div className="flex gap-2 items-center">
          <p className="font-semibold">{workout.name}</p>
          <StatusBadge status={workout.status} />
        </div>
        <p className="text-sm text-gray-500">
          {workout.totalExercises}{" "}
          {workout.totalExercises === 1 ? "exercise" : "exercises"}
        </p>
      </div>
      <Link
        to={`/workout/${workout.id}`}
        className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center rounded-md cursor-pointer"
      >
        View Workout
      </Link>
    </div>
  );
};

export default WorkoutListItem;
