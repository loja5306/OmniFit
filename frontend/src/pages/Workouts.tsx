import { useState } from "react";
import CreateWorkoutPanel from "../components/workouts/CreateWorkoutPanel";
import WorkoutListPanel from "../components/workouts/WorkoutListPanel";

const Workouts = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);

  return (
    <>
      <h1 className="text-3xl font-bold text-center pt-4">
        {isCreating ? "Create Workout" : "Your Workouts"}
      </h1>
      {isCreating ? (
        <CreateWorkoutPanel onClose={() => setIsCreating(false)} />
      ) : (
        <WorkoutListPanel onCreateWorkout={() => setIsCreating(true)} />
      )}
    </>
  );
};

export default Workouts;
