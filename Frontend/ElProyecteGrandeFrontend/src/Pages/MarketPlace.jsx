import { useState, useEffect } from 'react'

function MarketPlace() {

    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    async function FetchProducts() 
    {
        const response = await fetch("http://localhost:5036/Product/GetAllProducts");
        return await response.json();
    }

    useEffect(() => {
        FetchProducts()
        .then(products => setProducts(products), setLoading(false));

    }, [])

    return (<>
    {loading ? <h1>Loading...</h1> : 
    products.map(product => {return (
    <div className='productCard' key={product.id}>
        <p>{product.id}</p>
        <p>{product.seller.name}</p>
        <p>{product.price}</p>
        <p>{product.details}</p>
        <p>{product.quantity}</p>
    </div>
    )})
    }</>
    )

}

export default MarketPlace;