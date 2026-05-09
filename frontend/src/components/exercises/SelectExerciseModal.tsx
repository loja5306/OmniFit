import { useState } from "react";
import { useGetAllExercises } from "../../hooks/useGetAllExercises";
import type { Exercise } from "../../types/exerciseTypes";
import Modal from "../common/Modal";
import ExerciseSelectItem from "./ExerciseSelectItem";
import PaginationControl from "../common/PaginationControl";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSelectExercise: (exercise: Exercise) => void;
}

const SelectExerciseModal = ({ isOpen, onClose, onSelectExercise }: Props) => {
  const [page, setPage] = useState<number>(1);
  const { isPending, data } = useGetAllExercises({ page: page, pageSize: 5 });

  return (
    <Modal isOpen={isOpen} title="Select Exercise" onClose={onClose}>
      <div className="min-w-sm">
        {isPending || !data ? (
          <div>Pending...</div>
        ) : (
          <div className="space-y-2">
            {data.items.map((exercise: Exercise) => (
              <ExerciseSelectItem
                key={exercise.id}
                exercise={exercise}
                onSelectExercise={onSelectExercise}
              />
            ))}
            <PaginationControl
              page={page}
              totalPages={data.totalPages}
              onPageChange={setPage}
            />
          </div>
        )}
      </div>
    </Modal>
  );
};

export default SelectExerciseModal;
