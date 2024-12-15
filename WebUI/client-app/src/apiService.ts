import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL

export const login = async (name: string, password: string) => {
    try {
        const result = await axios.post(`${API_URL}/users/authenticate`, {
            name: name,
            password: password
        });

        return result.data;
    } catch (error) {
        console.error('Login failed:', error)
    }
}

export const uploadFile = async (file: File) => {
    const formData = new FormData()
    formData.append('file', file)

    try {
        const response = await axios.post(`${API_URL}/documents`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'User-Id': getUserId()
            },
        })

        return response.data
    } catch (error) {
        console.error('File upload failed:', error)
    }
}

export const uploadMultipleFiles = async (files: File[]) => {
    const formData = new FormData();

    files.forEach((file) => {
        formData.append('files', file)
    });

    try {
        const response = await axios.post(`${API_URL}/documents/upload-multiple`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'User-Id': getUserId()
            },
        })

        return response.data
    } catch (error) {
        console.error('Failed to upload files:', error)
    }
};

export const getDocuments = async () => {
    try {
        const response = await axios.get(`${API_URL}/documents`, {
            headers: {
                'User-Id': getUserId()
            },
        })

        return response.data
    } catch (error) {
        console.error('Failed to get documents:', error)
    }
};

export const downloadDocument = async (id: number, name: string) => {
    try {
        const response = await axios.get(`${API_URL}/documents/${id}/download`, {
            headers: {
                'User-Id': getUserId()
            },
            responseType: 'blob'
        })

        const fileURL = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = fileURL;
        link.setAttribute('download', name);
        document.body.appendChild(link);
        link.click();
    } catch (error) {
        console.error('Error downloading document', error);
    }
}

export const createSharedLink = async (id: number, durationInSeconds: number) => {
    try {
        const response = await axios.post(`${API_URL}/documents/${id}/share`,
            {
                durationInSeconds: durationInSeconds
            },
            {
                headers: {
                    'User-Id': getUserId()
                }
            })

        return response.data;
    } catch (error) {
        console.error('Error creating shared link', error);
    }
}

const getUserId = (): string => {
    return localStorage.getItem('userId')!
}