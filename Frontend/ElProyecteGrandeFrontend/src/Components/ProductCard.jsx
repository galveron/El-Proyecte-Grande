
function ProductCard(props) {
    const { product } = props

    return (
        <>
            <div className='productCard' key={product.id}>
                <div className='card-overlay'></div>
                <p>{product.id}</p>
                <p>Seller: {product.seller.name}</p>
                <p>Price: {product.price}$</p>
                <p>Description: {product.details}</p>
                <p>In stock: {product.quantity}</p>
            </div>
        </>
    )
}

export default ProductCard