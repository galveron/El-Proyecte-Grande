import { useState, useEffect } from "react";

import CustomerProfile from "../Components/Profile/CustomerProfile";
import CompanyProfile from "../Components/Profile/CompanyProfile";

function UserProfile() {
    const [user, setUser] = useState({});
    const [loading, setLoading] = useState(false);

    async function fetchUser() {
        let url = `http://localhost:5036/User/GetUser`;
        const res = await fetch(url,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        const data = await res.json();
        return data;
    }

    useEffect(() => {
        setLoading(true);
        fetchUser()
            .then(userData => setUser(userData), setLoading(false));
    }, []);


    if (loading) return <h2>Loading...</h2>;

    return (
        <div className="profile">
            <div className="user-profile-container">
                {user.company ? (
                    <CompanyProfile user={user} />
                ) : (
                    <div className="customer-profile">
                        <CustomerProfile user={user} />
                    </div>
                )}
            </div>
        </div>
    )
}

export default UserProfile;