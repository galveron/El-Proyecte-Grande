import { useState, useEffect } from "react";

import CustomerProfile from "../Components/Profile/CustomerProfile";
import CompanyProfile from "../Components/Profile/CompanyProfile";
import Unauthorized from "./Unauthorized/Unauthorized";

function UserProfile({ userRole }) {
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
        return data;
    }

    useEffect(() => {
        fetchUser()
        .then(data => setUser(data), setLoading(false))
    }, []);

    console.log(userRole);
    console.log(user);

    if (loading) return <h2>Loading...</h2>;

    if (userRole === "") return <Unauthorized />

    return (
        <div className="profile">
            <div className="user-profile-container">
                {user.company ? (
                    <CompanyProfile user={user} setUser={setUser} />
                ) : (
                    <div className="customer-profile">
                        <CustomerProfile user={user} setUser={setUser} />
                    </div>
                )}
            </div>
        </div>
    )
}

export default UserProfile;