import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import './index.css';
import Layout from './Pages/Layout/Layout';
import ErrorPage from './Pages/ErrorPage';
import Home from './Pages/Home';
import App from './App';
import MarketPlace from './Pages/MarketPlace';

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
      }
    ]
  }
])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
