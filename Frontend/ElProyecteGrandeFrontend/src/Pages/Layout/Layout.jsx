import { Outlet, Link } from "react-router-dom";

import './Layout.css';

const Layout = () => (
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
                        <li className="registration-layout">
                            <Link to="/registration">Registration</Link>
                        </li>
                    </ul>
                </nav>
            </div>
        </header>
        <Outlet />
    </>
)

export default Layout;