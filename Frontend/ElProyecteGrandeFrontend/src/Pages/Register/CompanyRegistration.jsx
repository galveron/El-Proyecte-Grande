import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';

notification.config({
    duration: 2,
    closeIcon: null
})

function CompanyRegistration() {
    const [userEmail, setUserEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [userPassword, setUserPassword] = useState('');
    const [companyName, setCompanyName] = useState('');
    const [identifier, setIdentifier] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/RegisterCompany', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Email: userEmail,
                    Password: userPassword,
                    Username: userName,
                    CompanyName: companyName,
                    Identifier: identifier
                })
            });
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
        <div className="register-comp">
            <div className="comp-register-container">
                <h3>Register as a company to sell your products</h3>
                <form className='comp-register' onSubmit={handleSubmit}>
                    <label>E-mail</label>
                    <input type='email' onChange={e => setUserEmail(e.target.value)} value={userEmail} required />
                    <label>Username</label>
                    <input type='text' onChange={e => setUserName(e.target.value)} value={userName} required />
                    <label>Password</label>
                    <input type='password' onChange={e => setUserPassword(e.target.value)} value={userPassword} required />
                    <label>Company's name</label>
                    <input type='text' onChange={e => setCompanyName(e.target.value)} value={companyName} required />
                    <label>Identifier</label>
                    <input type='text' onChange={e => setIdentifier(e.target.value)} value={identifier} required />
                    <button type='submit'>Register</button>
                </form>
            </div>
        </div>
    );
}

export default CompanyRegistration;