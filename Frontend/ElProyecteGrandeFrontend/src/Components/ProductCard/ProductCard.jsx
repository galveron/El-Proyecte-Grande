import './ProductCard.css';
import { notification } from 'antd';
import { useState } from 'react';

notification.config({
    duration: 2,
    closeIcon: null
})

function ProductCard({ product, user }) {
    const [isFavourite, setIsFavourite] = useState(false);

    async function addFavourite() {
        try {
            const res = await fetch(`http://localhost:5036/User/AddFavourite?productId=${product.id}`, {
                method: 'PATCH',
                credentials: 'include'
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            res.text()
            .then(text => {
                notification.info({message: `${text}`})
            })
            setIsFavourite(true);
        } catch (error) {
            throw error;
        }
    }

    async function removeFavourite() {
        try {
            const res = await fetch(`http://localhost:5036/User/RemoveFavourite?productId=${product.id}`, {
                method: 'PATCH',
                credentials: 'include'
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            res.text()
            .then(text => {
                notification.info({message: `${text}`})
            })
            setIsFavourite(false);
        } catch (error) {
            throw error;
        }
    }

    return (
        <div className='productCard'>
            <img className='img' src={product.image ? product.images.coverart : '/plant1.jpg'} />
            {user.company == null && (<button className='save' onClick={isFavourite ? removeFavourite : addFavourite}><i className="fa-regular fa-heart"></i></button>)}
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