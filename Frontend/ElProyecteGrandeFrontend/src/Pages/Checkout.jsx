import { useState, useEffect } from "react";

function Checkout(){

    const [user, setUser] = useState({});
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [country, setCountry] = useState("");
    const [city, setCity] = useState("");
    const [address, setAddress] = useState("");
    const [selected, setSelected] = useState(null);

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

    async function fetchPlaceOrder(){
        try{
            const url = 'http://localhost:5036/Order/AddOrder';
            const res = await fetch(url);
            const response = res.json();
        } 
        catch(error){
            throw error;
        }
    }

    useEffect(() => {
        fetchUser();
    },[])

    function onCheckboxChange(i) {
        setSelected((prev) => (i === prev ? null : i));
    }

    function placeOrder(e){
        e.preventdefault();
        fetchPlaceOrder();
    }

    return(
        <>
        <table>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Total Price</th>
                </tr>
            </thead>
            <tbody>
                {user.cartItems ? user.cartItems.map((item) => (<tr>
                    <td>{item.product.name}</td>
                    <td>{item.product.price}</td>
                    <td>{item.quantity}</td>
                    <td>{item.product.price * item.quantity}</td>
                </tr>)) : <></>}
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>{user.cartItems ? 
            user.cartItems.reduce((accumulator, currentValue) => 
            accumulator + (currentValue.product.price * currentValue.quantity), 0) : 0}</td>
                </tr>
            </tbody>
        </table>
        <section className="orderForm">
            <h2>Please fill in the details to finalize the order:</h2>
        <form >
            <label >First Name:
                <input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)}/>
            </label>
            <label >Last Name:
                <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)}/>
            </label>
            <label >Country:
                <input type="text" value={country} onChange={(e) => setCountry(e.target.value)}/>
            </label>
            <label >City:
                <input type="text" value={city} onChange={(e) => setCity(e.target.value)}/>
            </label>
            <label >Address:
                <input type="text" value={address} onChange={(e) => setAddress(e.target.value)}/>
            </label>
            <div className="paymentMethods">
            <label >Payment method:
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
            </label>
            </div>
        <button onSubmit={(e) => placeOrder(e)}>Place Order</button>
        </form>
        </section>
        </>
    )
}

export default Checkout;