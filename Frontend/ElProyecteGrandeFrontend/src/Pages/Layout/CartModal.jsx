import { useState, useEffect } from "react";

function CartModal({token, setIsShowCart, isShowCart, user}){
    const [customer, setCustomer] = useState({});

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
            setCustomer(data);
            return data;
        }
        catch (error) {
            throw error;
        }
    }

    useEffect(() => {
        if (token !== "" && user) {
            setCustomer(user);
        }
    }, [user])

    async function removeItemFromCart(productId, quantity){
        try{
            let url = `http://localhost:5036/User/AddOrRemoveCartItems?productId=${productId}&quantity=${quantity}`;
            await fetch(url,
                {
                    method: "PATCH",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            fetchUser();
        } 
        catch(error){
            throw error;
        }
    }

    function removeFromCart(productId, quantity){
        removeItemFromCart(productId, quantity * -1)
    }

    return(<>
    {isShowCart ?
        <section className="cartModal">
            <button className="closeModalButton" onClick={() => setIsShowCart(false)}>Close</button>
            {customer.cartItems && customer.cartItems.length > 0 ? customer.cartItems.map((item) => {
                return (<div key={item.id} className="cartItem">
                    <p>Name: {item.product.name}</p>
                    <p>Quantity: {item.quantity}</p>
                    <p>Price {item.quantity * item.product.price}</p>
                    <button onClick={() => removeFromCart(item.productId, item.quantity)}>Remove from Cart</button>
                </div>)
            }) : <></>}
            <p>Total Price: {customer.cartItems && customer.cartItems.length > 0? 
            customer.cartItems.reduce((accumulator, currentValue) => 
            accumulator + (currentValue.product.price * currentValue.quantity), 0) : 0} </p>
        </section> : <></>}
        </>)
}

export default CartModal;