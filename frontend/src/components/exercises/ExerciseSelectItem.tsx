import type { Exercise } from "../../types/exerciseTypes";

interface Props {
  exercise: Exercise;
  onSelectExercise: (exercise: Exercise) => void;
}

const ExerciseSelectItem = ({
  exercise,
  onSelectExercise,
}: Props) => {
  return (
    <button
      key={exercise.id}
      onClick={() => onSelectExercise(exercise)}
      className="w-full p-1 rounded-md shadow-lg border border-gray-200 cursor-pointer hover:bg-blue-50"
    >
      <h2 className="font-semibold text-lg">{exercise.name}</h2>
      <p className="text-md">{exercise.description}</p>
    </button>
  );
};

export default ExerciseSelectItem;
