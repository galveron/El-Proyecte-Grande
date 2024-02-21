function CustomerProfile({ user }) {
    return (
        <>
            <h2>Profile</h2>
            <label>Username</label>
            <p>{user.userName}</p>
            <label>E-mail address</label>
            <p>{user.email}</p>
            <label>Favourite products</label>
            {user.favourites && Array.isArray(user.favourites) ? (
                user.favourites.map((favProduct, index) => (
                    <div key={index} className="favourite-product-container">
                        <p>{favProduct.details || 'N/A'}</p>
                        <p>{favProduct.seller?.name || 'N/A'}</p>
                    </div>
                ))
            ) : (
                <p>No favourite products found</p>
            )}
        </>
    )
}

export default CustomerProfile;