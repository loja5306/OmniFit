import { Plus } from "lucide-react";
import { useGetWorkoutsForUser } from "../../hooks/useGetWorkoutsForUser";
import type { WorkoutResponse } from "../../types/workoutTypes";
import WorkoutListItem from "./WorkoutListItem";
import PaginationControl from "../common/PaginationControl";
import { useState } from "react";

interface Props {
  onCreateWorkout: () => void;
}

const WorkoutListPanel = ({ onCreateWorkout }: Props) => {
  const [page, setPage] = useState<number>(1);
  const { isPending, data } = useGetWorkoutsForUser({
    page: page,
    pageSize: 1,
  });

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
      ) : data ? (
        <div className="flex flex-col gap-2">
          {data.items.map((workout: WorkoutResponse) => (
            <div
              key={workout.id}
              className="p-3 rounded-md shadow-lg border border-gray-200"
            >
              <WorkoutListItem workout={workout} />
            </div>
          ))}
          <PaginationControl
            page={page}
            totalPages={data.totalPages}
            onPageChange={setPage}
          />
        </div>
      ) : null}
    </div>
  );
};

export default WorkoutListPanel;
