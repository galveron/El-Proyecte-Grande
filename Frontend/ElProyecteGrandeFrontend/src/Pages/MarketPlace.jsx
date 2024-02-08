import { useState, useEffect } from 'react'

function MarketPlace() {

    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    async function FetchProducts() {
        const response = await fetch("http://localhost:5036/Product/GetAllProducts");
        return await response.json();
    }

    useEffect(() => {
        FetchProducts()
            .then(products => setProducts(products), setLoading(false));

    }, [])

    return (<>
        {loading ? <h1>Loading...</h1> :
            <div className='productcard-container'>
                {products.map(product => {
                    return (
                        <div className='productCard' key={product.id}>
                            <div className='card-overlay'></div>
                            <p>{product.id}</p>
                            <p>Seller: {product.seller.name}</p>
                            <p>Price: {product.price}$</p>
                            <p>Description: {product.details}</p>
                            <p>In stock: {product.quantity}</p>
                        </div>
                    )
                })}
            </div>
        }</>
    )

}

export default MarketPlace;