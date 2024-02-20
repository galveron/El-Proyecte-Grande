import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import './index.css';
import Layout from './Pages/Layout/Layout';
import ErrorPage from './Pages/ErrorPage';
import Home from './Pages/Home';
import App from './App';
import MarketPlace from './Pages/MarketPlace';
import CustomerRegistration from './Pages/Register/CustomerRegistration';
import CompanyRegistration from './Pages/Register/CompanyRegistration';
import RegisterAs from './Pages/Register/Register';
import Login from './Pages/Login';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: '/',
        element: <Home />
      },
      {
        path: '/app',
        element: <App />
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
      }
    ]
  }
])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
