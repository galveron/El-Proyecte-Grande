import React from 'react';
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

async function fetchUser() {
  let url = `http://localhost:5036/User/GetUser`;
  const res = await fetch(url,
    {
      method: "GET",
      credentials: 'include',
      headers: {
        "Authorization": "Bearer token"
      }
    });
  const data = await res.json();
  console.log("data: " + data.userName)
  return data;
}

async function fetchRole() {
  const user = await fetchUser();
  let url = `http://localhost:5036/User/GetRole?userName=${user.userName}`;
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

let role = await fetchRole();
console.log("Role: " + role)

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout userRole={role} />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: '/',
        element: <Home userRole={role} />
      },
      {
        path: '/marketplace',
        element: <MarketPlace />
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
        element: <UserProfile />
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
        element: <AddProduct />
      }
    ]
  }
])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)

