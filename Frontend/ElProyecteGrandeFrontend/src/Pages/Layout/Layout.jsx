import { Outlet, Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';
import Cookies from "js-cookie";
import './Layout.css';

notification.config({
    duration: 2,
    closeIcon: null
})

function Layout() {
    const token = Cookies.get('token');
    const navigate = useNavigate();

    function handleLogout() {
        Cookies.remove('token');
        Cookies.remove('user_id');
        navigate('/');
        notification.success({ message: 'Logged out.' })
    }



    return (
        <>
            <header>
                <div className="header">
                    <nav>
                        <ul>
                            <li className="home-layout">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="products-layout">
                                <Link to="/marketplace">MarketPlace</Link>
                            </li>
                            {token ?
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