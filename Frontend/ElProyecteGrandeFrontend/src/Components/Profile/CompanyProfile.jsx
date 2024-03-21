import { useState, useEffect } from "react";
import EditCompany from "./EditCompany";

function CompanyProfile({ user, setUser }) {
    const [showEdit, setShowEdit] = useState(false);

    return (
        <div className="company-profile-container">
            <div className="company-profile">
                <h1>Profile</h1>
                <label>Username:</label>
                <p>{user.userName}</p>
                <label>E-mail address:</label>
                <p>{user.email}</p>
            </div>
            <div className="company-details">
                <h3>Company details</h3>
                <label>Company name:</label>
                <p>{user.company.name}</p>
                <label>Company identifier:</label>
                <p>{user.company.identifier}</p>
                <label>Verified company?</label>
                {user.company.verified ? (<p>Yes</p>) : (<p>No</p>)}
            </div>
            <br></br>
            <button className="edit-info" onClick={e => setShowEdit(true)}>Edit info</button>
            <EditCompany user={user} showEdit={showEdit} setShowEdit={setShowEdit} setUser={setUser} />
        </div>
    )
}

export default CompanyProfile;