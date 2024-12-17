import '../App.css'

interface LoadingSpinnerProps {
    isLoading:  boolean
}

const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({ isLoading }) => {
    if (!isLoading) {
        return null;
    }

    return (
        <>
            <div className='overlay'>
                <div className="spinner-border"></div>
            </div>
        </>
    )
}

export default LoadingSpinner
