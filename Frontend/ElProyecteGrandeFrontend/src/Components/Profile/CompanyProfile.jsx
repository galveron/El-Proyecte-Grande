function CompanyProfile({ user }) {
    return (
        <div className="company-profile-container">
            <div className="company-profile">
                <h2>Profile</h2>
                <label>Username</label>
                <p>{user.userName}</p>
                <label>E-mail address</label>
                <p>{user.email}</p>
            </div>
            <div className="company-details">
                <h3>Company details</h3>
                <label>Company name</label>
                <p>{user.company.name}</p>
                <label>Company identifier</label>
                <p>{user.company.identifier}</p>
                <label>Verified company?</label>
                {user.company.verified ? (<p>Yes</p>) : (<p>No</p>)}
            </div>
            <br></br>
            <button>Edit info</button>
        </div>
    )
}

export default CompanyProfile;