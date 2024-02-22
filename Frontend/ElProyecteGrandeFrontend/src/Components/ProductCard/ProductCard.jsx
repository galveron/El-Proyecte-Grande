import './ProductCard.css';
function ProductCard(props) {
    const { product } = props

    return (
        <>
            <div className='productCard' key={product.id}>
                <img className='img' src={product.image ? song.images.coverart : '/plant1.jpg'} />
                <button className='save'><i className="fa-regular fa-heart"></i></button>
                <div className='card-overlay'></div>
                <ul className='details'>
                    <li>{product.id}</li>
                    <li>Seller: {product.seller.name}</li>
                    <li>Price: {product.price}$</li>
                    <li>Description: {product.details}</li>
                    <li>In stock: {product.quantity}</li>
                </ul>

            </div>
        </>
    )
}

export default ProductCard