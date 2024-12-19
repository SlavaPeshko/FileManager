import React, { useState } from 'react';
import LoadingSpinner from './LoadingSpinner'
import SharedLinskModalWindow from './SharedLinksModalWindow'
import { DocumentDto } from '../types/DocumentDto'
import { DocumentType } from '../types/DocumentType'
import { downloadDocument } from '../apiService'
import { FaFilePdf, FaFileExcel, FaFileWord, FaFileAlt, FaFileImage, FaFile } from 'react-icons/fa'

import '../App.css'

interface DocumentTableProps {
    documents: DocumentDto[];
    onUpdateDocument: (updatedDocument: DocumentDto) => void;
}

const DocumentTable: React.FC<DocumentTableProps> = ({ documents, onUpdateDocument }) => {
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [isModalVisible, setIsModalVisible] = useState<boolean>(false)
    const [selectedDocumentId, setSelectedDocumentId] = useState<number | null>(null)

    const getDocumentIcon = (type: DocumentType) => {
        switch (type) {
            case DocumentType.Pdf:
                return <FaFilePdf className="text-danger" />;
            case DocumentType.Excel:
                return <FaFileExcel className="text-success" />;
            case DocumentType.Word:
                return <FaFileWord className="text-primary" />;
            case DocumentType.Txt:
                return <FaFileAlt className="text-secondary" />;
            case DocumentType.Picture:
                return <FaFileImage className="text-warning" />;
            default:
                return <FaFile className="text-muted" />;
        }
    };

    const handleDownload = async (document: DocumentDto) => {
        setIsLoading(true)
        await downloadDocument(document.id, document.name)
        document.downloadCount += 1
        onUpdateDocument(document)
        setIsLoading(false)
    };

    const handleOnOpenModalWindow = async (id: number) => {
        setIsModalVisible(true)
        setSelectedDocumentId(id)
    };

    const handleOnClose = () => {
        setIsModalVisible(false)
        setSelectedDocumentId(null)
    }

    return (
        <>
            <LoadingSpinner isLoading={isLoading} />
            <div className="container mt-4">
                <h3>Downloaded Documents</h3>
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Type</th>
                            <th>Preview</th>
                            <th>Uploaded At</th>
                            <th>Download Count</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {documents.length > 0 ? (
                            documents.map((document) => (
                                <tr key={document.id}>
                                    <td>{document.name}</td>
                                    <td>{getDocumentIcon(document.type)}</td>
                                    <td>{document.previewPath ?
                                        <img
                                            src={`${import.meta.env.VITE_API_URL}/${document.previewPath}`}
                                            alt={`${document.name} preview`}
                                            className="img-thumbnail"
                                        /> : <img alt="No preview available" />}</td>
                                    <td>{new Date(document.uploadAt).toLocaleString()}</td>
                                    <td>{document.downloadCount}</td>
                                    <td>
                                        <button style={{ marginRight: '5px' }} className="btn btn-primary btn-sm" onClick={() => handleDownload(document)}>Download</button>
                                        <button className="btn btn-primary btn-sm" onClick={() => handleOnOpenModalWindow(document.id)}>Create Share Link</button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={6} className="text-center">
                                    No documents available.
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
            {isModalVisible && <SharedLinskModalWindow onClose={handleOnClose} documentId={selectedDocumentId} />}
        </>

    );
};

export default DocumentTable;
