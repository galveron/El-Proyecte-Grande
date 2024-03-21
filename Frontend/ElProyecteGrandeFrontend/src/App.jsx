import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import './index.css';
import Layout from './Pages/Layout/Layout';
import ErrorPage from './Pages/ErrorPage';
import Home from './Pages/Home/Home';
import MarketPlace from './Pages/MarketPlace';
import CustomerRegistration from './Pages/Register/CustomerRegistration';
import CompanyRegistration from './Pages/Register/CompanyRegistration';
import RegisterAs from './Pages/Register/Register';
import Login from './Pages/Login';
import TestPage from './Pages/TestPage';
import UserProfile from './Pages/UserProfile';
import ImageUploader from './Pages/ImageUploader';
import AddProduct from './Pages/AddProduct';
import FavouriteProducts from './Pages/FavouriteProducts';
import EditUsers from './Pages/AdminPages/EditUsers';
import EditCompanies from './Pages/AdminPages/EditCompanies';
import EditProducts from './Pages/AdminPages/EditProducts';
import Checkout from './Pages/Checkout';

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

function App() {

    let [userRole, setUserRole] = useState("")
    let [user, setUser] = useState();
    let cookie = getCookie('User')

    async function fetchUser() {
        let url = `http://localhost:5036/User/GetUser`;
        if (cookie.length !== 0) {
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include'
                });
            const data = await res.json();
            setUser(data)
            return data;
        }
        return ""
    }

    async function fetchRole() {
        const user = await fetchUser();
        if (user.length !== 0) {

            let url = `http://localhost:5036/User/GetRole?id=${user.id}`;
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include',
                    headers: {
                        "Authorization": "Bearer token"
                    }
                });
            const data = await res.text();
            return data;
        }
        return ""
    }

    useEffect(() => {
        fetchRole()
            .then(role => setUserRole(role))

    }, [])


    const router = createBrowserRouter([
        {
            path: '/',
            element: <Layout userRole={userRole} user={user} />,
            errorElement: <ErrorPage />,
            children: [
                {
                    path: '/',
                    element: <Home userRole={userRole} />
                },
                {
                    path: '/marketplace',
                    element: <MarketPlace userRole={userRole} setCustomer={setUser} />
                },
                {
                    path: '/register',
                    element: <RegisterAs />
                },
                {
                    path: '/login',
                    element: <Login />
                },
                {
                    path: '/register-customer',
                    element: <CustomerRegistration />
                },
                {
                    path: '/register-company',
                    element: <CompanyRegistration />
                }, {
                    path: '/profile',
                    element: <UserProfile userRole={userRole} />
                },
                {
                    path: '/test-page',
                    element: <TestPage />
                },
                {
                    path: '/upload-image',
                    element: <ImageUploader />
                },
                {
                    path: '/add-product',
                    element: <AddProduct userRole={userRole} />
                },
                {
                    path: '/favourites',
                    element: <FavouriteProducts userRole={userRole} />
                },
                {
                    path: '/edit-users',
                    element: <EditUsers />
                },
                {
                    path: '/edit-companies',
                    element: <EditCompanies />
                },
                {
                    path: '/edit-products',
                    element: <EditProducts />
                },
                {
                    path: '/checkout',
                    element: <Checkout />
                }
            ]
        }
    ])
    return <RouterProvider router={router} />;
}

export default App; 