import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from 'js-cookie';
import { notification } from 'antd';

notification.config({
    duration: 2,
    closeIcon: null
})

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/Login', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: email, Password: password })
            });
            if (!res.ok) {
                notification.error({ message: 'Email or password incorrect!' });
                throw new Error(`HTTP error! status: ${res.status}`);
            }

            navigate('/');
            notification.success({ message: 'Successful login. Welcome!' });
            window.location.reload()
        }
        catch (error) {
            throw error;
        }
    }

    return (
        <div className="login">
            <div className="login-container">
                <form className='login' onSubmit={e => handleLogin(e)}>
                    <label>E-mail</label>
                    <input type='text' onChange={e => setEmail(e.target.value)} value={email} required />
                    <label>Password</label>
                    <input type='password' onChange={e => setPassword(e.target.value)} value={password} required />
                    <button type='submit'>Login</button>
                </form>
            </div>
        </div>
    );
}

export default Login;