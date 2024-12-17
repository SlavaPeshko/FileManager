import React, { useState } from 'react'
import LoadingSpinner from './LoadingSpinner'
import { uploadMultipleFiles } from '../apiService'

interface UploadFilesProps {
    onDocumentsUpload: () => void;
}

const validTypes = ['.pdf', '.xls', '.xlsx', '.doc', '.docx', '.txt', '.jpg', '.jpeg', '.png', '.gif', '.bmp', '.png']
const maxFileSize = 5 * 1024 * 1024

const UploadFiles: React.FC<UploadFilesProps> = ({ onDocumentsUpload }) => {
    const [files, setFiles] = useState<File[]>([])
    const [error, setError] = useState<string>('')
    const [isLoading, setIsLoading] = useState<boolean>(false)

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const selectedFiles = event.target.files
        if (!selectedFiles) return

        const fileArray: File[] = Array.from(selectedFiles)
        setError('')

        const invalidFiles = fileArray.filter((file) => {
            const fileExtension = file.name.slice(file.name.lastIndexOf('.')).toLowerCase();
            return !validTypes.includes(fileExtension) || file.size > maxFileSize;
        })

        if (invalidFiles.length > 0) {
            setError('Some files are invalid.')
            return
        }

        setFiles((prevFiles) => [...prevFiles, ...fileArray])
    };

    const handleSubmit = async (): Promise<void> => {
        if (files.length === 0) {
            setError('Please select files to upload.')
            return
        }
        setIsLoading(true)

        await uploadMultipleFiles(files)
        onDocumentsUpload()

        setFiles([])
        setError('')
        setIsLoading(false)
    };

    return (
        <>
            <LoadingSpinner isLoading={isLoading} />
            <div className="container mt-5">
                <h3>Upload Multiple Files</h3>

                {error && <div className="alert alert-danger">{error}</div>}

                <div className="mb-3">
                    <label htmlFor="fileUpload" className="form-label">Choose Files</label>
                    <input
                        id="fileUpload"
                        type="file"
                        className="form-control"
                        multiple
                        accept="application/pdf, image/png, image/jpeg, .xls, .xlsx, .doc, .docx, .txt, .gif, .bmp"
                        onChange={handleFileChange}
                    />
                </div>

                {files.length > 0 && (
                    <div className="mb-3">
                        <h5>Selected Files:</h5>
                        <ul className="list-group">
                            {files.map((file, index) => (
                                <li key={index} className="list-group-item">
                                    {file.name} - {Math.round(file.size / 1024)} KB
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                <button className="btn btn-primary" onClick={handleSubmit}>
                    Upload Files
                </button>
            </div>
        </>

    );
};

export default UploadFiles
