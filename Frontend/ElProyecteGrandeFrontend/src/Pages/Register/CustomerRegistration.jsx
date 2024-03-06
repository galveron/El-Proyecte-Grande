import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';

notification.config({
    duration: 2,
    closeIcon: null
})

function CustomerRegistration() {
    const [userEmail, setUserEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [userPassword, setUserPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/Register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: userEmail, Username: userName, Password: userPassword })
            });
            const data = await res.json();
            if (!res.ok) {
                throw new Error(`${data[Object.keys(data)[0]][0]}`);
            }
            navigate('/');
        }
        catch (error) {
            notification.error({ message: `Couldn't register. ${error.message}` });
        }
    }

    return (
        <div className="register-cust">
            <div className="cust-register-container">
                <form className='cust-register' onSubmit={handleSubmit}>
                    <label>E-mail</label>
                    <input type='email' onChange={e => setUserEmail(e.target.value)} value={userEmail} required />
                    <label>Username</label>
                    <input type='text' onChange={e => setUserName(e.target.value)} value={userName} required />
                    <label>Password</label>
                    <input type='password' onChange={e => setUserPassword(e.target.value)} value={userPassword} required />
                    <button type='submit'>Register</button>
                </form>
            </div>
        </div>
    );
}

export default CustomerRegistration;