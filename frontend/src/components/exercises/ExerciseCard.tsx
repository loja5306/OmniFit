import type { Exercise } from "../../types/exerciseTypes";

interface Props {
  exercise: Exercise;
}

const ExerciseCard = ({ exercise }: Props) => {
  return (
    <li key={exercise.id} className="bg-white p-4 rounded-lg shadow-lg">
      <h2 className="text-center font-bold text-xl pb-2">{exercise.name}</h2>
      <p className="text-center">{exercise.description}</p>
    </li>
  );
};

export default ExerciseCard;
