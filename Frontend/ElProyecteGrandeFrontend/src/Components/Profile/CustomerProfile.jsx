import { useState } from "react";
import EditCustomer from "./EditCustomer";

function CustomerProfile({ user, setUser }) {
    const [showEdit, setShowEdit] = useState(false);
    
    return (
        <>
            <h2>Profile</h2>
            <label>Username</label>
            <p>{user.userName}</p>
            <label>E-mail address</label>
            <p>{user.email}</p>
            <label>Phone number</label>
            <p>{user !== null ? (user.phoneNumber !== null ? user.phoneNumber : "No phone number") : ""}</p>
            <br></br>
            <button onClick={e => setShowEdit(true)}>Edit info</button>
            <EditCustomer user={user} showEdit={showEdit} setShowEdit={setShowEdit} setUser={setUser}/>
        </>
    )
}

export default CustomerProfile;