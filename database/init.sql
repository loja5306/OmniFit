-- Creating Workouts Table

CREATE TABLE IF NOT EXISTS "Workouts" (
  "Id" UUID PRIMARY KEY,
  "CreatedOn" TIMESTAMPTZ NOT NULL,
  "UpdatedOn" TIMESTAMPTZ NOT NULL,
  "Name" TEXT NOT NULL
);

-- Creating Exercises Table

CREATE TABLE IF NOT EXISTS "Exercises" (
  "Id" UUID PRIMARY KEY,
  "CreatedOn" TIMESTAMPTZ NOT NULL,
  "UpdatedOn" TIMESTAMPTZ NOT NULL,
  "Name" TEXT NOT NULL,
  "Description" TEXT NOT NULL
);

-- Creating WorkoutExercises Table

CREATE TABLE IF NOT EXISTS "WorkoutExercises" (
  "Id" UUID PRIMARY KEY,
  "CreatedOn" TIMESTAMPTZ NOT NULL,
  "UpdatedOn" TIMESTAMPTZ NOT NULL,
  "WorkoutId" UUID NOT NULL REFERENCES "Workouts"("Id"),
  "ExerciseId" UUID NOT NULL REFERENCES "Exercises"("Id")
);

-- Creating WorkoutSet Table

CREATE TABLE IF NOT EXISTS "WorkoutSets" (
  "Id" UUID PRIMARY KEY,
  "CreatedOn" TIMESTAMPTZ NOT NULL,
  "UpdatedOn" TIMESTAMPTZ NOT NULL,
  "SetNumber" INT NOT NULL,
  "Reps" INT NOT NULL,
  "Weight" DECIMAL(5,2) NOT NULL,
  "WorkoutExerciseId" UUID NOT NULL REFERENCES "WorkoutExercises"("Id")
);