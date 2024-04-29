import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';

function Checkout() {

    const [user, setUser] = useState({});
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [country, setCountry] = useState("");
    const [city, setCity] = useState("");
    const [address, setAddress] = useState("");
    const [selected, setSelected] = useState(null);
    const [orderSent, setOrderSent] = useState(false)

    const paymentOptions = ["Cash on delivery", "Bank transfer", "card payment"];


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
            setUser(data);
            return data;
        }
        catch (error) {
            throw error;
        }
    }

    async function fetchPlaceOrder() {
        try {
            const url = 'http://localhost:5036/Order/AddOrder';
            const res = await fetch(url,
                {
                    method: "POST",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
            return await res.json();
        }
        catch (error) {
            throw error;
        }
    }

    async function addProductToOrder(orderId, productId, quantity) {
        try {
            const url = `http://localhost:5036/Order/AddOrRemoveProductFromOrder?orderId=${orderId}&productId=${productId}&quantity=${quantity}`;
            const res = await fetch(url,
                {
                    method: "POST",
                    credentials: 'include',
                    headers: { "Authorization": "Bearer token" }
                });
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

    function clearCart() {
        clearCartItems();
        notification.success({ message: 'Order is successful!' });
        setOrderSent(true)
    }
    useEffect(() => {
        fetchUser();
    }, [])

    function onCheckboxChange(i) {
        setSelected((prev) => (i === prev ? null : i));
    }

    async function placeOrder(e) {
        e.preventDefault();
        console.log("SUBMIT")
        const orderID = await fetchPlaceOrder();
        for (let item of user.cartItems) {
            await addProductToOrder(orderID, item.product.id, item.quantity);
        }
    }

    return (
        <>
            {!orderSent ?
                <div className="checkout-page">
                    <div className="checkout-table-div">
                        <table className="checkout-table">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Price</th>
                                    <th>Quantity</th>
                                    <th>Total Price</th>
                                </tr>
                            </thead>
                            <tbody>
                                {user.cartItems ? user.cartItems.map((item) => (<tr key={item.product.id}>
                                    <td>{item.product.name}</td>
                                    <td>{item.product.price}Ft</td>
                                    <td>{item.quantity}db</td>
                                    <td>{item.product.price * item.quantity}Ft</td>
                                </tr>)) : <></>}
                                <tr className="empty-row">

                                </tr>
                                <tr>
                                    <td style={{ fontWeight: '900' }}>Total</td>
                                    <td></td>
                                    <td></td>
                                    <td style={{ fontWeight: '900' }}>{user.cartItems ?
                                        user.cartItems.reduce((accumulator, currentValue) =>
                                            accumulator + (currentValue.product.price * currentValue.quantity), 0) : 0}Ft</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <section className="orderForm">
                        <h2>Please fill in the details to finalize the order:</h2>

                        <table className='checkout-details-table'>
                            <tbody className='checkout-details-tbody'>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>First Name:</td>
                                    <td className='checkout-details-td'><input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} /></td>
                                </tr>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>Last Name:</td>
                                    <td className='checkout-details-td'>
                                        <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} />
                                    </td>
                                </tr>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>Country:</td>
                                    <td className='checkout-details-td'>
                                        <input type="text" value={country} onChange={(e) => setCountry(e.target.value)} />
                                    </td>
                                </tr>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>City:</td>
                                    <td className='checkout-details-td'>
                                        <input type="text" value={city} onChange={(e) => setCity(e.target.value)} />
                                    </td>
                                </tr>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>Address:</td>
                                    <td className='checkout-details-td'>
                                        <input type="text" value={address} onChange={(e) => setAddress(e.target.value)} />
                                    </td>
                                </tr>
                                <tr className='checkout-details-tr'>
                                    <td style={{ fontWeight: '400' }} className='checkout-details-td'>Payment method:</td>
                                    <td className='checkout-details-td'>
                                        {paymentOptions.map((o, i) => (
                                            <label key={i}>
                                                {o}
                                                <input
                                                    type="checkbox"
                                                    checked={i === selected}
                                                    onChange={() => onCheckboxChange(i)}
                                                />
                                            </label>
                                        ))}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <button className="save-order" onClick={e => { placeOrder(e), clearCart(), setOrderSent(true) }}>Place order</button>
                    </section>
                </div >
                : <div className="checkout-page">
                    <h1>Your order was successful! You'll get an email soon with your order's details</h1>
                </div>
            }
        </>
    )
}

export default Checkout;