import { Check, Clock } from "lucide-react";

interface Props {
  status: "InProgress" | "Completed";
}

const StatusBadge = ({ status }: Props) => {
  const isCompleted = status === "Completed";
  const statusText = isCompleted ? "Completed" : "In Progress";

  return (
    <span
      className={`flex gap-1 items-center border-2 rounded-lg px-2 py-1 text-xs font-medium ${
        isCompleted
          ? "bg-green-50 border-green-200 text-green-700"
          : "bg-amber-50 border-amber-200 text-amber-700"
      }`}
    >
      {isCompleted ? (
        <Check className="w-3 h-3" />
      ) : (
        <Clock className="w-3 h-3" />
      )}
      <span>{statusText}</span>
    </span>
  );
};
export default StatusBadge;
