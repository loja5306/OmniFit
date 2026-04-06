import { X } from "lucide-react"

interface Props {
  isOpen: boolean;
  title: string;
  children: React.ReactNode;
  onClose: () => void;
}

const Modal = ({ isOpen, title, children, onClose} : Props) => {
  if (!isOpen) return null;
  
  return (
    <div className="bg-black/50 fixed inset-0 flex justify-center items-center">
      <div className="bg-white p-4 space-y-4 rounded-lg">
        <div className="flex justify-between items-center gap-2">
          <h2 className="text-xl font-bold">{title}</h2>
          <button onClick={onClose} className="cursor-pointer">
            <X size={20} />
          </button>
        </div>
        {children}
      </div>
    </div>
  )
}

export default Modal