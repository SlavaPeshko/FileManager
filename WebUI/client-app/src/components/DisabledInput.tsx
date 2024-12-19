import '../App.css'

interface DisabledInputProps {
    label: string;
    text: string;
}

const DisabledInput: React.FC<DisabledInputProps> = ({ label, text }) => {
    return (
        <>
            <label className="form-label">{label}</label>
            <input value={text} disabled={true} type="text" className="form-control" />
        </>
    )
}

export default DisabledInput
