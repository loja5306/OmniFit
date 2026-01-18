import { useState, type FormEvent } from "react";
import type { AddExerciseModalProps } from "../types/propTypes";
import { useCreateExercise } from "../hooks/useCreateExercise";
import { Plus, X } from "lucide-react";

const AddExerciseModal = ({ isOpen, onClose }: AddExerciseModalProps) => {
  const [name, setName] = useState<string>("");
  const [description, setDescription] = useState<string>("");

  const createExercise = useCreateExercise();

  if (!isOpen) return null;

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      await createExercise.mutateAsync({ name, description });
      setName("");
      setDescription("");
      onClose();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="bg-black/50 fixed inset-0 flex justify-center items-center">
      <form className="bg-slate-100 p-4 space-y-4" onSubmit={handleSubmit}>
        <h2 className="text-center text-xl font-bold">Create Exercise</h2>
        <div className="flex justify-between gap-2">
          <label>Name</label>
          <input
            className="bg-white border"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </div>
        <div className="flex justify-between gap-2">
          <label>Description</label>
          <input
            className="bg-white border"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <div className="flex justify-center gap-4">
          <button
            className="flex items-center gap-1 bg-red-600 py-1 px-3 rounded-lg shadow-lg text-white text-lg font-bold cursor-pointer"
            type="button"
            onClick={onClose}
          >
            <X size={20} strokeWidth={4} />
            <span>Cancel</span>
          </button>
          <button
            disabled={createExercise.isPending}
            className="flex items-center gap-1 bg-green-400 py-1 px-3 rounded-lg shadow-lg text-white text-lg font-bold cursor-pointer"
            type="submit"
          >
            <Plus size={20} strokeWidth={4} />
            <span>Create</span>
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddExerciseModal;
