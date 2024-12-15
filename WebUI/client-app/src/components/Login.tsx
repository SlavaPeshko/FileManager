import { useState } from 'react'
import { login } from '../apiService'

interface LoginProps {
    setLoggedIn:  (value: boolean) => void;
}

function Login(props: LoginProps) {
    const {  setLoggedIn } = props
    const [name, setName] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState<string>('')

    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setError('')

        const userId = await login(name, password)
        if (userId > 0) {
            localStorage.setItem('userId', userId)
            setLoggedIn(true)

            return
        }

        setError('Invalid username or password')
    }

    return (
        <>
            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={handleLogin}>
                <div className="mb-3">
                    <label className="form-label" htmlFor="userName">Name</label>
                    <input
                        type="text"
                        className="form-control"
                        id="userName"
                        placeholder="Enter name"
                        value={name}
                        onChange={(e: React.FormEvent<HTMLInputElement>) => setName(e.currentTarget.value)} />

                </div>
                <div className="mb-3">
                    <label className="form-label" htmlFor="password">Password</label>
                    <input
                        type="password"
                        className="form-control"
                        id="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e: React.FormEvent<HTMLInputElement>) => setPassword(e.currentTarget.value)} />
                </div>
                <button type="submit" className="btn btn-primary">Login</button>
            </form>
        </>
    )
}

export default Login
