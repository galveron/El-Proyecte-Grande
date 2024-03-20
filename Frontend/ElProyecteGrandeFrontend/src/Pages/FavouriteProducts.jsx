import React from "react";
import { useEffect, useState } from "react";
import Products from "../Components/Products/Products";
import Unauthorized from "./Unauthorized/Unauthorized";

const FavouriteProducts = ({ userRole }) => {
    const [user, setUser] = useState({});
    const [loading, setLoading] = useState(true);

    async function fetchUser() {
        let url = `http://localhost:5036/User/GetUser`;
        const res = await fetch(url,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        const data = await res.json()
        .then(user => setUser(user), setLoading(false));
        return data;
    }

    const handleSetUser = (data) => {
        setUser(data);
    }

    useEffect(() => {
        fetchUser()
    }, [])

    if (userRole === "") return <Unauthorized />;

    return (<>
        <div className='marketplace'>
            {loading ? <h1>Loading...</h1> :
                <Products products={user.favourites} user={user} handleSetUser={handleSetUser} />
            }
        </div>
    </>
    )
}

export default FavouriteProducts;