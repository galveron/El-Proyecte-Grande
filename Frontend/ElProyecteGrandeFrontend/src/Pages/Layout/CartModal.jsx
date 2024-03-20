import { useState, useEffect } from "react";

function CartModal({token, setIsShowCart, isShowCart}){

    const [cartItems, setCartItems] = useState([]);

    async function fetchUser() {
        try {
            let url = `http://localhost:5036/User/GetUser`;
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            const data = await res.json();
            setCartItems(data.cartItems);
            return data;
        }
        catch (error) {
            throw error;
        }
    }

    useEffect(() => {
        if (token !== "") {
            fetchUser();
        }
    }, [])

    return(<>
    {isShowCart ?
        <section className="cartModal">
            <button className="closeModalButton" onClick={() => setIsShowCart(false)}>Close</button>
            {cartItems ? cartItems.map((item) => {
                return (<div key={item.id} className="cartItem">
                    <p>Name: {item.product.name}</p>
                    <p>Quantity: {item.quantity}</p>
                    <p>Price {item.quantity * item.product.price}</p>
                </div>)
            }) : <></>}
            <p>Total Price: {cartItems && cartItems.length > 0? 
            cartItems.reduce((accumulator, currentValue) => 
            (accumulator.product.price * accumulator.quantity) + (currentValue.product.price * currentValue.quantity)) : 0} </p>
        </section> : <></>}
        </>)
}

export default CartModal;