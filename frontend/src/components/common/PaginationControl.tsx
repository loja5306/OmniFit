interface Props {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

const PaginationControl = ({ page, totalPages, onPageChange }: Props) => {
  const groupSize = 5;
  const startPage = Math.floor((page - 1) / groupSize) * groupSize + 1;
  const endPage = Math.min(startPage + groupSize - 1, totalPages);
  const pages = Array.from(
    { length: endPage - startPage + 1 },
    (_, i) => startPage + i,
  );

  return (
    <div className="flex justify-center items-center gap-2 mt-4">
      <button
        onClick={() => onPageChange(page - 1)}
        disabled={page <= 1}
        className="px-3 py-1.5 rounded text-sm font-medium cursor-pointer hover:bg-gray-100 disabled:hover:bg-transparent disabled:text-gray-300 disabled:cursor-not-allowed"
      >
        Prev
      </button>
      {startPage > 1 && <span>...</span>}
      {pages.map((p) => (
        <button
          key={p}
          onClick={() => onPageChange(p)}
          className={`px-3 py-1.5 rounded text-sm font-medium cursor-pointer ${
            p === page
              ? "bg-blue-500 text-white"
              : "text-gray-600 hover:bg-gray-100"
          }`}
        >
          {p}
        </button>
      ))}
      {endPage < totalPages && <span>...</span>}
      <button
        onClick={() => onPageChange(page + 1)}
        disabled={page >= totalPages}
        className="px-3 py-1.5 rounded text-sm font-medium cursor-pointer hover:bg-gray-100 disabled:hover:bg-transparent disabled:text-gray-300 disabled:cursor-not-allowed"
      >
        Next
      </button>
    </div>
  );
};
export default PaginationControl;
