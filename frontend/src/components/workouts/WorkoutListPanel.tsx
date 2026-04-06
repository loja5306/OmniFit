import { Plus } from "lucide-react";
import { useGetWorkoutsForUser } from "../../hooks/useGetWorkoutsForUser";
import type { WorkoutResponse } from "../../types/workoutTypes";

interface Props {
  onCreateWorkout: () => void;
}

const WorkoutListPanel = ({ onCreateWorkout }: Props) => {
  const { isPending, data } = useGetWorkoutsForUser();

  return (
    <div className="max-w-lg mx-auto bg-white mt-6 p-4 rounded-lg shadow-md">
      <div className="w-full flex justify-end items-center pb-4">
        <button
          onClick={onCreateWorkout}
          className="bg-blue-400 text-white font-semibold py-1 px-2 flex items-center gap-2 rounded-md cursor-pointer"
        >
          <Plus size={20} strokeWidth={3} />
          Start Workout
        </button>
      </div>
      {isPending ? (
        <div>Loading...</div>
      ) : (
        <div className="flex flex-col gap-2">
          {data.map((workout: WorkoutResponse) => (
            <div
              key={workout.id}
              className="p-3 rounded-md shadow-lg border border-gray-200"
            >
              <div>
                <p className="font-semibold">{workout.name}</p>
                <p className="text-sm text-gray-500">
                  {workout.totalExercises}{" "}
                  {workout.totalExercises === 1 ? "exercise" : "exercises"}
                </p>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default WorkoutListPanel;
