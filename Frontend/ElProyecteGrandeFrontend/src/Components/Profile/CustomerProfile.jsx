function CustomerProfile({ user }) {
    return (
        <>
            <h2>Profile</h2>
            <label>Username</label>
            <p>{user.userName}</p>
            <label>E-mail address</label>
            <p>{user.email}</p>
            <br></br>
            <button>Edit info</button>
        </>
    )
}

export default CustomerProfile;