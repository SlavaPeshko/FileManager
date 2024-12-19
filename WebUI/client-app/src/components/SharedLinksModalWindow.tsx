import { useState } from 'react'
import DisabledInput from './DisabledInput'
import LoadingSpinner from './LoadingSpinner'
import { formatIsoDate } from '../utils/helper'

import { createSharedLink } from '../apiService'

interface SharedLinksModalWindowProps {
    onClose: () => void;
    documentId: number | null;
}

const SharedLinksModalWindow: React.FC<SharedLinksModalWindowProps> = ({ onClose, documentId }) => {
    const [timeExpires, setTimeExpires] = useState<number>(0)
    const [sharedLink, setSharedLink] = useState<string>('')
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [formattedTime, setFormattedTime] = useState<string>('')

    const handleChange = (event: React.FormEvent<HTMLSelectElement>) => {
        setTimeExpires(Number(event.currentTarget.value))
    }

    const handleOnGenerate = async () => {
        if (!documentId || timeExpires === 0) {
            return
        }

        setIsLoading(true)
        const response = await createSharedLink(documentId, timeExpires)
        setSharedLink(response.sharedLink)
        setFormattedTime(formatIsoDate(response.expirationDate))
        setIsLoading(false)
    };

    return (
        <>
            <LoadingSpinner isLoading={isLoading} />
            <div className="modal fade show" style={{ display: 'block' }}>
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Create Shared Link</h5>
                            <button onClick={() => onClose()} type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <div className="mb-3">
                                {sharedLink ? <>
                                    <DisabledInput text={formattedTime} label='Expires On' />
                                    <DisabledInput text={sharedLink} label='Link' />
                                </> : <>
                                    <select value={timeExpires} onChange={handleChange} className="form-select">
                                        <option value="0" disabled>Choose time ...</option>
                                        <option value="300">5 Minut</option>
                                        <option value="1800">30 Minut</option>
                                        <option value="3600">1 Hour</option>
                                    </select>
                                </>}
                            </div>
                        </div>
                        <SharedLinksModalWindowFooter onClose={onClose} generate={handleOnGenerate} sharedLink={sharedLink} />
                    </div>
                </div>
            </div>
        </>
    )
}

interface SharedLinksModalWindowFooterProps {
    onClose: () => void;
    generate: () => void;
    sharedLink: string;
}

const SharedLinksModalWindowFooter: React.FC<SharedLinksModalWindowFooterProps> = ({ onClose, generate, sharedLink }) => {
    const [copied, setCopied] = useState(false)

    const handleOnCopy = () => {
        navigator.clipboard.writeText(sharedLink)
        setCopied(true)
    }

    return (
        <>
            <div className="modal-footer">
                <button onClick={() => onClose()} type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                {!sharedLink ? <button onClick={generate} type="button" className="btn btn-primary">Generate</button>
                    : <button onClick={handleOnCopy} type="button" className="btn btn-primary">{!copied ? "Copy link" : "Copied!"}</button>}

            </div>
        </>
    )
}

export default SharedLinksModalWindow
