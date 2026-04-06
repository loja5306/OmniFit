import { Plus } from "lucide-react";
import { useGetAllExercises } from "../../hooks/useGetAllExercises";
import type { Exercise } from "../../types/exerciseTypes";
import AddExerciseModal from "./CreateExerciseModal";
import { useState } from "react";
import ExerciseCard from "./ExerciseCard";

const ExerciseGrid = () => {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const { isPending, data } = useGetAllExercises();

  if (isPending) return <div>Pending...</div>;

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
        {data.map((exercise: Exercise) => (
          <ExerciseCard key={exercise.id} exercise={exercise} />
        ))}
      </ul>
    </div>
  );
};

export default ExerciseGrid;
