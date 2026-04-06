import { useGetAllExercises } from "../../hooks/useGetAllExercises";
import type { Exercise } from "../../types/exerciseTypes";
import Modal from "../common/Modal";
import ExerciseSelectItem from "./ExerciseSelectItem";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSelectExercise: (exercise: Exercise) => void;
}

const SelectExerciseModal = ({
  isOpen,
  onClose,
  onSelectExercise,
}: Props) => {
  const { isPending, data } = useGetAllExercises();

  return (
    <Modal isOpen={isOpen} title="Select Exercise" onClose={onClose}>
      <div className="min-w-sm">
        {isPending ? (
          <div>Pending...</div>
        ) : (
          <div className="space-y-2">
            {data.map((exercise: Exercise) => (
              <ExerciseSelectItem
                key={exercise.id}
                exercise={exercise}
                onSelectExercise={onSelectExercise}
              />
            ))}
          </div>
        )}
      </div>
    </Modal>
  );
};

export default SelectExerciseModal;
