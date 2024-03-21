import { useState, useEffect } from "react";
import { Outlet, Link } from "react-router-dom";
import { Button, Modal } from 'flowbite-react';
import '../../index.css'

function CartModal({ token, setIsShowCart, isShowCart, user }) {
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

    async function removeItemFromCart(productId, quantity) {
        try {
            let url = `http://localhost:5036/User/AddOrRemoveCartItems?productId=${productId}&quantity=${quantity}`;
            await fetch(url,
                {
                    method: "PATCH",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            fetchUser();
        }
        catch (error) {
            throw error;
        }
    }

    async function clearCartItems() {
        try {
            let url = 'http://localhost:5036/User/EmptyCart';
            await fetch(url,
                {
                    method: "PATCH",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            fetchUser();
        }
        catch (error) {
            throw error;
        }
    }

    async function clearCartItems() {
        try {
            let url = 'http://localhost:5036/User/EmptyCart';
            await fetch(url,
                {
                    method: "PATCH",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            fetchUser();
        }
        catch (error) {
            throw error;
        }
    }

    function removeFromCart(productId, quantity) {
        removeItemFromCart(productId, quantity * -1)
    }

    function clearCart() {
        clearCartItems();
    }

    return (<>
        <Modal className="cartModal" dismissible show={isShowCart} onClose={() => setIsShowCart(false)}>
            <Modal.Body className='cart-modal-body'>
                <div className='modal-header'>
                    <button className="closeModalButton" onClick={() => setIsShowCart(false)}>X</button>
                </div>
                {customer.cartItems && customer.cartItems.length > 0 ? customer.cartItems.map((item) => {
                    return (<div key={item.id} className="cartItem">
                        <div><p><span style={{ fontWeight: '400' }}>
                            Name: </span> {item.product.name} | <span style={{ fontWeight: '600' }}>
                                Price: </span> {item.quantity * item.product.price}Ft | <span style={{ fontWeight: '600' }}>
                                Quantity: </span> {item.quantity}db</p><p></p></div>
                        <button className="remove-from-cart" onClick={() => removeFromCart(item.productId, item.quantity)}>Remove</button>
                        <div className="line"></div>
                    </div>)
                }) : <></>}
                <p style={{ fontWeight: '600' }}>Total Price: {customer.cartItems && customer.cartItems.length > 0 ?
                    customer.cartItems.reduce((accumulator, currentValue) =>
                        accumulator + (currentValue.product.price * currentValue.quantity), 0) : 0}Ft </p>
                <button onClick={() => clearCart()} className="clear-cart">Clear Cart</button>

                <a to='/checkout'><button className="go-checkout" onClick={() => setIsShowCart(false)}>Go to Checkout</button></a>
            </Modal.Body>
        </Modal>
    </>)
}

export default CartModal;