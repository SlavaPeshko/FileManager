import { useState, useEffect } from 'react'
import './App.css'
import Login from './components/Login'
import UploadDocuments from './components/UploadFiles'
import DocumentTable from './components/DocumentTable'
import { getDocuments } from './apiService'
import { DocumentDto } from './types/DocumentDto'

function App() {
  const [loggedIn, setLoggedIn] = useState<boolean>(false);
  const [documents, setDocuments] = useState<DocumentDto[]>([]);

  useEffect(() => {
    const userId = localStorage.getItem("userId") || '0'
    if (userId === '0') {
      return
    }

    setLoggedIn(true);
    getData()

    async function getData() {
      const data = await getDocuments()
      setDocuments(data.items)
    }
  }, []);

  const handleDocumentsUpload = async () => {
    const data = await getDocuments()
    setDocuments(data.items)
  };

  const handleDocumentUpdate = (updatedDocument: DocumentDto) => {
    setDocuments((prevDocuments) =>
      prevDocuments.map((document) =>
        document.id === updatedDocument.id ? { ...document, ...updatedDocument } : document
      )
    )
  }

  return (
    <>
      {loggedIn ? (
        <>
          <UploadDocuments onDocumentsUpload={handleDocumentsUpload} />
          <DocumentTable documents={documents} onUpdateDocument={handleDocumentUpdate} />
        </>
      ) : (
        <Login setLoggedIn={setLoggedIn} />
      )}
    </>
  )
}

export default App
