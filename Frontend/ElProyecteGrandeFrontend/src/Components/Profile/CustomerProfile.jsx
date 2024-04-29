import { useState } from "react";
import EditCustomer from "./EditCustomer";

function CustomerProfile({ user, setUser }) {
    const [showEdit, setShowEdit] = useState(false);

    return (
        <>
            <h1>Profile</h1>
            <label>Username:</label>
            <p>{user.userName}</p>
            <label>E-mail address:</label>
            <p>{user.email}</p>
            <label>Phone number:</label>
            <p>{user !== null ? (user.phoneNumber !== null ? user.phoneNumber : "No phone number") : ""}</p>
            <br></br>
            <button className="edit-info" onClick={e => setShowEdit(true)}>Edit info</button>
            <EditCustomer user={user} showEdit={showEdit} setShowEdit={setShowEdit} setUser={setUser} />
        </>
    )
}

export default CustomerProfile;