import { Outlet, Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';
import './Layout.css';

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

function Layout() {
    let isToken = getCookie('User');
    const navigate = useNavigate();

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
        }
        catch (error) {
            throw error;
        }
    }

    return (
        <>
            <header className="header-container">
                <div className="header">
                    <nav>
                        <ul>
                            <li className="home-layout">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="products-layout">
                                <Link to="/marketplace">MarketPlace</Link>
                            </li>
                            {isToken != "" ?
                                <>
                                    <li className="profile-layout">
                                        <Link to="/profile">Profile</Link>
                                    </li>
                                    <li className="logout-layout" onClick={handleLogout}>
                                        <Link to="/">Logout</Link>
                                    </li>
                                </> :
                                <>
                                    <li className="registration-layout">
                                        <Link to="/register">Registration</Link>
                                    </li>
                                    <li className="login-layout">
                                        <Link to="/login">Login</Link>
                                    </li>
                                </>
                            }
                        </ul>
                    </nav>
                </div>
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
                            <li><Link to="/faq">FAQ</Link></li>
                            <li><Link to="/termsandpolicy">Terms & Policy</Link></li>
                        </ul>
                    </nav>
                </div>
            </footer>
        </>
    )
}

export default Layout;