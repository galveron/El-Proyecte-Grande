import Cookies from 'js-cookie';
import { useState, useEffect } from 'react';


function TestPage() {

    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [loggedIn, setLoggedIn] = useState(false);
    const [userId, setUserId] = useState("");
    const [user, setUser] = useState({});

    async function FetchProducts() {
        const response = await fetch("http://localhost:5036/Product/GetAllProducts");
        return await response.json();
    }

    async function FetchUserData() {
        const response = await fetch(`http://localhost:5036/User/GetUser?id=${userId}`);
        const customer = await response.json();
        console.log(customer);
        setUser(customer);
        console.log(user)
    }

    useEffect(() => {
        FetchProducts()
            .then(products => setProducts(products), setLoading(false));
        console.log(Cookies.get("user_id"))
        if (Cookies.get("user_id")) {
            setUserId(Cookies.get("user_id").replace(/^"|"$/g, ''))
            setLoggedIn(true);
        }
    }, [])

    useEffect(() => {
        console.log(loggedIn)
        if (loggedIn) {
            FetchUserData()
        }
    }, [loggedIn])

    async function AddToCart(productId) {
        await fetch(`http://localhost:5036/User/AddOrRemoveCartItems?userId=${userId}&productId=${productId}&quantity=1`, {
            method: "PATCH",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${Cookies.get('token').replace(/^"|"$/g, '')}`
            }
        })
        FetchUserData();
    }

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
                            {loggedIn ? <button onClick={() => AddToCart(product.id)}>Add to Cart</button> : <></>}
                        </div>
                    )
                })}
                <div>
                    <h2>Cart: </h2>
                    {user && user.cartItems ? user.cartItems.map(item => {
                        return (
                            <div className='productCard' key={item.productId}>
                                <div className='card-overlay'></div>
                                <p>{item.productId}</p>
                                <p>Quantity: {item.quantity}</p>
                            </div>)
                    })
                        : <></>}
                </div>
            </div>

        }</>
    )

}

export default TestPage;
