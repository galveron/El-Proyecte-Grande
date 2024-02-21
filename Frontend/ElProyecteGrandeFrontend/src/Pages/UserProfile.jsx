import { useState, useEffect } from "react";
import Cookies from "js-cookie";

import CustomerProfile from "../Components/Profile/CustomerProfile";
import CompanyProfile from "../Components/Profile/CompanyProfile";

function UserProfile() {
    const [user, setUser] = useState({});
    const [loading, setLoading] = useState(false);
    const id = Cookies.get('user_id');

    async function fetchUser() {
        let url = `http://localhost:5036/User/GetUser?id=${id.replace(/^"|"$/g, '')}`;
        const res = await fetch(url);
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
        <div className="user-profile-container">
            {user.company ? (
                <CompanyProfile user={user} />
            ) : (
                <div className="customer-profile">
                    <CustomerProfile user={user} />
                </div>
            )}
        </div>
    )
}

export default UserProfile;