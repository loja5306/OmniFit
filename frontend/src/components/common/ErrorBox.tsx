interface Props {
  errorMessages: string[];
}

const ErrorBox = ({ errorMessages }: Props) => {
  return (
    <div className="bg-red-50 mb-2 rounded-xl p-2 border-2 border-red-200 text-red-700 text-sm">
      {typeof errorMessages === "string"
        ? errorMessages
        : errorMessages.map((message) => <p key={message}>{message}</p>)}
    </div>
  );
};
export default ErrorBox;
