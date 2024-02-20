import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import './index.css';
import Layout from './Pages/Layout/Layout';
import ErrorPage from './Pages/ErrorPage';
import Home from './Pages/Home';
import App from './App';
import MarketPlace from './Pages/MarketPlace';
import CustomerRegistration from './Pages/CustomerRegistration';
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
        element: <CustomerRegistration />
      },
      {
        path: '/login',
        element: <Login />
      }
    ]
  }
])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
