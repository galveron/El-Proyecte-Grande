import './Home.css';

const Home = ({ userRole }) => (
    <div className="home-page">
        <img src="/background1.jpg" />
        <h2 className='welcome'>Welcome {userRole} to our website!<br />Feel free to check out our Marketplace and order something for yourself!</h2>
    </div>
)

export default Home;