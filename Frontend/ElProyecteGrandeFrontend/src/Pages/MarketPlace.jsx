import { useState, useEffect } from 'react'
import Products from '../Components/Products/Products';

function MarketPlace() {
    const [loading, setLoading] = useState(true);
    const [products, setProducts] = useState([]);
    const [user, setUser] = useState({});

    async function fetchProducts() {
        const response = await fetch("http://localhost:5036/Product/GetAllProducts");
        return await response.json();
    }

    async function fetchUser() {
        let url = `http://localhost:5036/User/GetUser`;
        const res = await fetch(url,
            {
                method: "GET",
                credentials: 'include',
                headers: { "Authorization": "Bearer token" }
            });
        const data = await res.json();
        return data;
    }

    useEffect(() => {
        fetchProducts()
            .then(products => setProducts(products), setLoading(false));
    }, [])

    return (<>
        <div className='marketplace'>
            {loading ? <h1>Loading...</h1> :
                <Products products={products} user={user} />
            }
        </div>
    </>
    )
}

export default MarketPlace;