import React, { useState } from 'react';
import { DocumentDto } from '../types/DocumentDto'
import { DocumentType } from '../types/DocumentType'
import { downloadDocument } from '../apiService'
import { createSharedLink } from '../apiService'
import { FaFilePdf, FaFileExcel, FaFileWord, FaFileAlt, FaFileImage, FaFile } from 'react-icons/fa'

interface DocumentTableProps {
    documents: DocumentDto[];
    onUpdateDocument: (updatedDocument: DocumentDto) => void;
}

const DocumentTable: React.FC<DocumentTableProps> = ({ documents, onUpdateDocument }) => {
    const [sharedLink, setSharedLink] = useState<string>('')

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
        await downloadDocument(document.id, document.name)
        document.downloadCount += 1
        onUpdateDocument(document)
    };

    const handleShare = async (id: number) => {
        const sharedLink = await createSharedLink(id, 240)
        setSharedLink(sharedLink)
    };

    return (
        <div className="container mt-4">
            {sharedLink && <div className="alert alert-primary">{sharedLink}</div>}
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
                                        style={{ width: '50px', height: '50px', objectFit: 'cover' }}
                                    /> : <img alt="No preview available" />}</td>
                                <td>{new Date(document.uploadAt).toLocaleString()}</td>
                                <td>{document.downloadCount}</td>
                                <td>
                                    <button style={{ marginRight: '5px' }} className="btn btn-primary btn-sm" onClick={() => handleDownload(document)}>Download</button>
                                    <button className="btn btn-primary btn-sm" onClick={() => handleShare(document.id)}>Share</button>
                            </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={5} className="text-center">
                                No documents available.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default DocumentTable;
