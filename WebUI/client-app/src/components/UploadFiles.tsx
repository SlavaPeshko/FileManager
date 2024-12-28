import React, { useEffect, useState } from 'react'
import LoadingSpinner from './LoadingSpinner'
import { uploadMultipleFiles } from '../apiService'
import { getUserIdFromLocalStorage } from '../utils/helper'
import * as signalR from '@microsoft/signalr'

interface UploadFilesProps {
    onDocumentsUpload: () => void
}

const validTypes = ['.pdf', '.xls', '.xlsx', '.doc', '.docx', '.txt', '.jpg', '.jpeg', '.png', '.gif', '.bmp', '.png']

const UploadFiles: React.FC<UploadFilesProps> = ({ onDocumentsUpload }) => {
    const [files, setFiles] = useState<File[]>([])
    const [error, setError] = useState<string>('')
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [connection, setConnection] = useState<signalR.HubConnection>()
    const [connectionId, setConnectionId] = useState<string>('')
    const [uploadStatusByFile, setUploadStatusByFile] = useState<Record<string, number>>({})

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_API_URL}/fileUploadHub`, {
                headers: {
                    'User-Id': getUserIdFromLocalStorage()
                }
            })
            .withAutomaticReconnect()
            .build()

            setConnection(newConnection);
    }, [])

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    connection.invoke('GetConnectionId').then(value => {
                        setConnectionId(value)
                    }).catch(error =>{
                        console.log(error)
                    })
                    connection.on('ReceiveProgress', (fileName, percentage) => {
                        setUploadStatusByFile(prevState => ({
                            ...prevState,
                            [fileName]: percentage
                        }))
                    })
                })
        }
    }, [connection])

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const selectedFiles = event.target.files
        if (!selectedFiles) return

        const fileArray = Array.from(selectedFiles)
        setError('')

        const invalidFiles = fileArray.filter((file) => {
            const fileExtension = file.name.slice(file.name.lastIndexOf('.')).toLowerCase()
            return !validTypes.includes(fileExtension)
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

        if(connection === undefined) {
            return
        }

        await uploadMultipleFiles(files.filter(file => !uploadStatusByFile[file.name]), connectionId)
        onDocumentsUpload()

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
                                    {file.name} - {Math.round(file.size / 1024)} KB, 
                                    <ProgressUploadingFile progress={uploadStatusByFile[file.name] || 0}/>
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

interface ProgressUploadingFileProps {
    progress: number;
}

const ProgressUploadingFile : React.FC<ProgressUploadingFileProps> = ({ progress }) => {
    const progressColor = progress === 100 ? 'green' : 'blue'

    return (
        <span style={{ color: progressColor, marginLeft: '5px' }}>
            downloading: {progress}%
        </span>
    );
}
