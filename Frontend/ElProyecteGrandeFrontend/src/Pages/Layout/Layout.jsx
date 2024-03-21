import { Outlet, Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';
import CartModal from "./CartModal";
import { useState } from "react";
import './Layout.css';
import { Button, Modal } from 'flowbite-react';

notification.config({
    duration: 2,
    closeIcon: null
})


function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}

function Layout({ userRole, user }) {
    let isToken = getCookie('User');
    const navigate = useNavigate();
    const [showRegisterCustomer, setShowRegisterCustomer] = useState(false)
    const [showRegisterCompany, setShowRegisterCompany] = useState(false)
    const [showLogin, setShowLogin] = useState(null)
    const [customerUserName, setCustomerUserName] = useState(null)
    const [customerEmail, setCustomerEmail] = useState(null)
    const [customerPassword, setCustomerPassword] = useState(null)
    const [showRegister, setShowRegister] = useState(false)
    const [companyUserName, setCompanyUserName] = useState(null)
    const [companyEmail, setCompanyEmail] = useState(null)
    const [companyPassword, setCompanyPassword] = useState(null)
    const [companyName, setCompanyName] = useState(null)
    const [companyIdentifier, setCompanyIdentifier] = useState(null)
    const [isShowCart, setIsShowCart] = useState(false);
    const [loginEmail, setLoginEmail] = useState('');
    const [loginPassword, setLoginPassword] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/Login', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: loginEmail, Password: loginPassword })
            });
            if (!res.ok) {
                notification.error({ message: 'Email or password incorrect!' });
                throw new Error(`HTTP error! status: ${res.status}`);
            }

            navigate('/');
            notification.success({ message: 'Successful login. Welcome!' });
            setTimeout(function () {
                window.location.reload();
            }, 2000);
        }
        catch (error) {
            throw error;
        }
    }
    async function handleLogout() {
        try {
            const res = await fetch('http://localhost:5036/Auth/Logout', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            isToken = "";
            navigate('/');
            notification.success({ message: 'Logged out.' })
            setTimeout(function () {
                window.location.reload();
            }, 2000);
        }
        catch (error) {
            throw error;
        }
    }

    const handleSaveCustomer = async e => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/Register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: customerEmail, Username: customerUserName, Password: customerPassword })
            });
            const data = await res.json();
            if (!res.ok) {
                throw new Error(`${data[Object.keys(data)[0]][0]}`);
            }
            navigate('/');
            notification.success({ message: 'Successful registration!' });
        }
        catch (error) {
            notification.error({ message: `Couldn't register. ${error.message}` });
        }
    }

    const handleSaveCompany = async e => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5036/Auth/RegisterCompany', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Email: companyEmail,
                    Password: companyPassword,
                    Username: companyUserName,
                    CompanyName: companyName,
                    Identifier: companyIdentifier
                })
            });
            const data = res.json();
            if (!res.ok) {
                throw new Error(`${data[Object.keys(data)[0]][0]}`);
            }
            navigate('/');
            notification.success({ message: 'Successful registration!' });
        }
        catch (error) {
            notification.error({ message: `Couldn't register. ${error.message}` });
        }
    }

    function clickOnCart() {
        isShowCart ? setIsShowCart(false) : setIsShowCart(true);
    }

    return (
        <>
            <header className="header-container">
                <div className="header">
                    {userRole === "Admin" ?
                        <nav>
                            <ul>
                                <li className="edit-users-layout">
                                    <Link className="nav-link" to="/edit-users">Users</Link>
                                </li>
                                <li className="edit-companies-layout">
                                    <Link className="nav-link" to="/edit-companies">Companies</Link>
                                </li>
                                <li className="edit-products-layout">
                                    <Link className="nav-link" to="/edit-products">Products</Link>
                                </li>
                                <li className="logout-layout" onClick={handleLogout}>
                                    <Link className="nav-link" to="/">Logout</Link>
                                </li>
                            </ul>
                        </nav>
                        : <nav>
                            <ul>
                                {/* <li className="home-layout">
                                    <Link className="nav-link" to="/">Home</Link>
                                </li> */}
                                <li className="products-layout">
                                    <Link className="nav-link" to="/marketplace">MarketPlace</Link>
                                </li>
                                {isToken != "" ?
                                    <>
                                        <li className="profile-layout">
                                            <Link className="nav-link" to="/profile">Profile</Link>
                                        </li>
                                        <li className="logout-layout" onClick={handleLogout}>
                                            <Link className="nav-link" to="/">Logout</Link>
                                        </li>
                                    </> :
                                    <>
                                        <li className="registration-layout">
                                            <div className="nav-link" id="register-link" onClick={e => setShowRegister(true)}>Registration</div>
                                        </li>
                                        <li className="login-layout">
                                            <Link className="nav-link" onClick={e => setShowLogin(true)}>Login</Link>
                                        </li>
                                    </>
                                }
                                {userRole === "Customer" && <li className="favourites">
                                    <Link className="nav-link" to="/favourites">
                                        <i className="fa-solid fa-heart" id="layout-heart"></i>
                                    </Link>
                                </li>}
                                {userRole === "Customer" && <button className="cart-button" onClick={() => clickOnCart()}><i className="fa-solid fa-cart-shopping"></i></button>}
                            </ul>
                        </nav>}
                </div>
                <CartModal token={isToken} setIsShowCart={setIsShowCart} isShowCart={isShowCart} user={user} />
            </header >
            <Outlet />
            <footer>
                <div className="footer">
                    <nav className="row">
                        <ul className="column">
                            <li>Phone: +56/2345677</li>
                            <li>Email: jungle@jungle.com</li>
                            <li>Address: 2345, Jungle str. 345.</li>
                        </ul>
                        <ul className="column">
                            <li><Link className="nav-link" to="/faq">FAQ</Link></li>
                            <li><Link className="nav-link" to="/termsandpolicy">Terms & Policy</Link></li>
                        </ul>
                    </nav>
                </div>
            </footer>
            <Modal className='login-modal' dismissible show={showLogin} onClose={() => setShowLogin(false)}>
                <Modal.Body className='login-modal-body'>
                    <div className='modal-header'>
                        <h1>Login</h1>
                        <button className='close-modal' onClick={e => setShowLogin(false)}>X</button>
                    </div>
                    <div>
                        <h3>Email:</h3>
                        <input className="login-input" type="text" onChange={e => setLoginEmail(e.target.value)} value={loginEmail}></input>
                        <h3>Password:</h3>
                        <input type="password" className="login-input" onChange={e => setLoginPassword(e.target.value)} value={loginPassword}></input>
                    </div>
                    <button className="confirm" onClick={e => { handleLogin(e), setShowLogin(false) }}>Log in</button>
                </Modal.Body>
            </Modal>
            <Modal className='register-modal' dismissible show={showRegister} onClose={() => setShowRegister(false)}>
                <Modal.Body className='register-modal-bigbody'>
                    <div className='register-modal-header'>
                        <h1>Register as... </h1>
                        <button className='close-modal' onClick={e => setShowRegister(false)}>X</button>
                    </div>
                    <div className='register-modal-body'>
                        <button className='choose-customer' onClick={e => { setShowRegisterCustomer(true), setShowRegister(false) }}>a Customer</button>
                        <button className='choose-company' onClick={e => { setShowRegisterCompany(true), setShowRegister(false) }}>a Company</button>
                    </div>
                </Modal.Body>
            </Modal>
            <Modal className='login-modal' dismissible show={showRegisterCustomer} onClose={() => setShowRegisterCustomer(false)}>
                <Modal.Body className='login-modal-body'>
                    <div className='modal-header'>
                        <h1>Register</h1>
                        <button className='close-modal' onClick={e => setShowRegisterCustomer(false)}>X</button>
                    </div>
                    <div>
                        <h3>User name:</h3>
                        <input className="login-input" type="text" onChange={e => setCustomerUserName(e.target.value)} value={customerUserName}></input>
                        <h3>Email:</h3>
                        <input className="login-input" type="text" onChange={e => setCustomerEmail(e.target.value)} value={customerEmail}></input>
                        <h3>Password:</h3>
                        <input type="password" className="login-input" onChange={e => setCustomerPassword(e.target.value)} value={customerPassword}></input>
                    </div>
                    <button className="confirm" onClick={e => { handleSaveCustomer(e), setShowRegisterCustomer(false) }}>Save</button>
                </Modal.Body>
            </Modal>
            <Modal className='login-modal' dismissible show={showRegisterCompany} onClose={() => setShowRegisterCompany(false)}>
                <Modal.Body className='login-modal-body'>
                    <div className='modal-header'>
                        <h1>Register</h1>
                        <button className='close-modal' onClick={e => setShowRegisterCompany(false)}>X</button>
                    </div>
                    <div>
                        <h3>User name:</h3>
                        <input className="login-input" type="text" onChange={e => setCompanyUserName(e.target.value)} value={companyUserName}></input>
                        <h3>Email:</h3>
                        <input className="login-input" type="text" onChange={e => setCompanyEmail(e.target.value)} value={companyEmail}></input>
                        <h3>Password:</h3>
                        <input type="password" className="login-input" onChange={e => setCompanyPassword(e.target.value)} value={companyPassword}></input>
                        <h3>Company's name:</h3>
                        <input className="login-input" type="text" onChange={e => setCompanyName(e.target.value)} value={companyName}></input>
                        <h3>Identifier:</h3>
                        <input className="login-input" type="text" onChange={e => setCompanyIdentifier(e.target.value)} value={companyIdentifier}></input>
                    </div>
                    <button className="confirm" onClick={e => { handleSaveCompany(e), setShowRegisterCompany(false) }}>Save</button>
                </Modal.Body>
            </Modal>
        </>
    )
}

export default Layout;