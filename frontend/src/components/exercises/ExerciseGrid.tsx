import { Plus } from "lucide-react";
import { useGetAllExercises } from "../../hooks/useGetAllExercises";
import AddExerciseModal from "./CreateExerciseModal";
import { useState } from "react";
import ExerciseCard from "./ExerciseCard";
import PaginationControl from "../common/PaginationControl";

const ExerciseGrid = () => {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [page, setPage] = useState<number>(1);
  const { isPending, data } = useGetAllExercises({ page: page, pageSize: 12 });

  if (isPending || !data) return <div>Pending...</div>;

  return (
    <div className="p-4">
      <AddExerciseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
      />
      <div className="flex justify-end">
        <button
          className="flex items-center gap-1 bg-blue-400 py-2 px-4 rounded-lg shadow-lg text-white text-xl font-bold cursor-pointer"
          onClick={() => setIsModalOpen(true)}
        >
          <Plus size={24} strokeWidth={4} />
          <span>Add</span>
        </button>
      </div>
      <ul className="grid grid-cols-3 gap-4 pt-2">
        {data.items.map((exercise) => (
          <ExerciseCard key={exercise.id} exercise={exercise} />
        ))}
      </ul>
      <PaginationControl
        page={page}
        totalPages={data.totalPages}
        onPageChange={setPage}
      />
    </div>
  );
};

export default ExerciseGrid;
