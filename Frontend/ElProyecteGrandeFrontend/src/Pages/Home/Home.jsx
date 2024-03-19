import './Home.css';

const Home = ({ userRole }) => (
    <div className="home-page">
        <img src="/background1.jpg" />
        {userRole === "Admin" ?
            <h2 className='welcome'>Hello Admin! <br />Do not forget to check the companies waiting for validation!</h2>
            : <h2 className='welcome'>Welcome to E-Jungle {userRole}! <br />Feel free to check out our Marketplace and order a nice plant for yourself!</h2>
        }
    </div>
)

export default Home;