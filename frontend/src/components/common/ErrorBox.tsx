import { RotateCw } from "lucide-react";

interface Props {
  errorMessages: string[];
  onRetry?: () => void;
}

const ErrorBox = ({ errorMessages, onRetry }: Props) => {
  return (
    <div className="bg-red-50 mb-2 rounded-xl p-2 border-2 border-red-200 text-red-700 text-sm">
      {errorMessages.map((message) => (
        <p key={message}>{message}</p>
      ))}
      {onRetry && (
        <button
          onClick={onRetry}
          className="mt-2 flex items-center gap-1 rounded-md border border-red-700 bg-white px-2 py-0.5 text-xs font-semibold text-red-700 hover:bg-red-100 cursor-pointer"
        >
          <RotateCw size={12} />
          Retry
        </button>
      )}
    </div>
  );
};
export default ErrorBox;
