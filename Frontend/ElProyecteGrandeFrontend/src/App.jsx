import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [forecasts, setForecasts] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchForeCast = async () => {
    try
    {
      const res = await fetch("http://localhost:5036/WeatherForecast");
      const data = await res.json();
      return data;
    }
    catch (error)
    {
      throw error;
    }
  }

  useEffect(() => {
    fetchForeCast().then(data => {
      setForecasts(data);
      setLoading(false);
    })
  }, []);

  return (
    <>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      {loading ? <h2>Loading...</h2> : forecasts.map(forecast => {
        return <p>{forecast.date}</p>
      })}
      <div className="card">
        <p>
          Edit <code>src/App.jsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
