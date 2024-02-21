function CompanyProfile({ user }) {
    return (
        <>
            <h2>Profile</h2>
            <label>Username</label>
            <p>{user.userName}</p>
            <label>E-mail address</label>
            <p>{user.email}</p>
            <h3>Company details</h3>
            <label>Company name</label>
            <p>{user.company.name}</p>
            <label>Company identifier</label>
            <p>{user.company.identifier}</p>
            <label>Verified company?</label>
            {user.company.verified ? (<p>Yes</p>) : (<p>No</p>)}
        </>
    )
}

export default CompanyProfile;