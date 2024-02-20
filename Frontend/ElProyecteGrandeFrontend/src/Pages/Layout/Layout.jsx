import { Outlet, Link } from "react-router-dom";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';

notification.config({
  duration: 2,
  closeIcon: null
})

import './Layout.css';

function Layout() {
    const token = Cookies.get('token');
    const navigate = useNavigate();

    function handleLogout() {
        Cookies.remove('token');
        navigate('/');
        notification.success({ message: 'Logged out.' })
    }

    return (
        <>
            <header>
                <div className="layout">
                    <nav>
                        <ul>
                            <li className="home-layout">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="products-layout">
                                <Link to="/marketplace">MarketPlace</Link>
                            </li>
                            {token ?
                                <li className="logout-layout" onClick={handleLogout}>
                                    <Link to="/">Logout</Link>
                                </li> :
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
        </>
    )
}

export default Layout;