import { useState, useEffect } from 'react'
import Products from '../Components/Products/Products';

function MarketPlace() {

    const [loading, setLoading] = useState(true);
    const [products, setProducts] = useState([]);


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
            <Products products={products} />
        }</>
    )

}

export default MarketPlace;