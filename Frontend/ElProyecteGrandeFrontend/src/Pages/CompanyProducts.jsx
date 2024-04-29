import { useState, useEffect } from 'react'
import Products from '../Components/Products/Products';

function CompanyProducts({ userRole, setCustomer }) {
    const [loading, setLoading] = useState(true);
    const [products, setProducts] = useState([]);
    const [user, setUser] = useState({});

    async function fetchProducts() {
        const response = await fetch(`http://localhost:5036/Product/GetCompaniesProducts?userName=${user.userName}`);
        return await response.json();
    }

    async function fetchUser() {
        if (userRole !== "") {
            let url = `http://localhost:5036/User/GetUser`;
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include',
                    headers: { 'Content-type': 'application/json' }
                });
            const data = await res.json();
            return data;
        }
    }

    const handleSetUser = (data) => {
        setUser(data);
    }

    useEffect(() => {
        fetchUser()
            .then(user => setUser(user))
            .then(fetchProducts()
                .then(products => setProducts(products), setLoading(false)));
    }, [])

    return (<>
        <div className='marketplace'>
            {loading ? <h1>Loading...</h1> :
                <Products {...{ products, user, handleSetUser, userRole, setCustomer }} />
            }
        </div>
    </>
    )
}

export default CompanyProducts;