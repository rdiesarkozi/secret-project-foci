import { FormEvent, useState } from 'react'
import './App.css'

function App() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        if (!email.trim() || !password.trim()) {
            setError('Please enter both email and password.')
            return
        }

        setError('')
        console.log('Login submitted', { email, password })
    }

    return (
        <div className="login-page">
            <form className="login-card" onSubmit={handleSubmit}>
                <h1>Sign in</h1>
                <p className="login-subtitle">Enter your credentials to continue</p>

                <label htmlFor="email">Email</label>
                <input
                    id="email"
                    type="email"
                    placeholder="name@example.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />

                <label htmlFor="password">Password</label>
                <input
                    id="password"
                    type="password"
                    placeholder="Enter your password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                {error && <p className="error-text">{error}</p>}

                <button type="submit">Login</button>
            </form>
        </div>
    )
}

export default App