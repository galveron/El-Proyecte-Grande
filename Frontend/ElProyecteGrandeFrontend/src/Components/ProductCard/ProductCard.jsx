import './ProductCard.css';
function ProductCard(props) {
    const { product } = props
    const company = props.product.seller.company
    return (
        <div className='productCard'>
            <img className='img' src={product.image ? product.images.coverart : '/plant1.jpg'} />
            <button className='save'><i className="fa-regular fa-heart"></i></button>
            <p>{product.name}</p>
            <div className='details-container'>
                <ul className='details-title'>
                    <li> </li>
                    <li>Seller:</li>
                    <li>Price:</li>
                    <li>In stock:</li>
                </ul>
                <ul className='details-data'>
                    <li>{product.seller.company ? product.seller.company.name : "na"}</li>
                    <li>{product.price}$</li>
                    <li>{product.quantity}</li>
                </ul>
            </div>
        </div>
    )
}

export default ProductCard