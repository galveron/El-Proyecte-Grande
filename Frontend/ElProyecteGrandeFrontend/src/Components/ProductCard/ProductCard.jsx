import './ProductCard.css';
import { notification } from 'antd';
import { useState, useEffect } from 'react';

notification.config({
    duration: 2,
    closeIcon: null
})

function ProductCard({ product, user, handleSetUser, userRole, setCustomer }) {

    function isProductInFavourites() {
        if (user && user.favourites) {
            return user.favourites.some(fav => fav.id === product.id);
        }
    }

    const [isFavourite, setIsFavourite] = useState(undefined);

    useEffect(() => {
        setIsFavourite(isProductInFavourites());
    })

    async function fetchUser() {
        if (user) {
            let url = `http://localhost:5036/User/GetUser`;
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include',
                    headers: { 'Content-type': 'application/json' }
                });
            const data = await res.json();
            return data;
        }
    }

    async function addFavourite() {
        if (user) {
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
                        notification.info({ message: `${text}` })
                    })
                setIsFavourite(true);
                let userAfterAddFav = await fetchUser();
                handleSetUser(userAfterAddFav);
            } catch (error) {
                throw error;
            }
        }
        else {
            notification.warning({ message: "Login first to favourite this item!" })
        }
    }

    async function removeFavourite() {
        if (user) {
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
                        notification.info({ message: `${text}` })
                    })
                setIsFavourite(false);
                let userAfterRemoveFav = await fetchUser();
                handleSetUser(userAfterRemoveFav);
            } catch (error) {
                throw error;
            }
        }
        else {
            notification.warning({ message: "Login first to favourite this item!" })
        }
    }

    async function addToCart() {
        if (user) {
            try {
                const res = await fetch(`http://localhost:5036/User/AddOrRemoveCartItems?productId=${product.id}&quantity=1`, {
                    method: 'PATCH',
                    credentials: 'include'
                });
                if (!res.ok) {
                    throw new Error(`HTTP error! status: ${res.status}`);
                }
                res.text()
                    .then(text => {
                        notification.info({ message: `${text}` })
                    })
                setCustomer(await fetchUser())
            } catch (error) {
                throw error;
            }
        }
        else {
            notification.warning({ message: "Login first to buy this item!" })
        }
    }

    return (
        <div className='productCard'>
            <img className='img' src={product.image ? product.images.coverart : '/plant1.jpg'} />
            {(userRole === "Customer" || userRole === "") && (<button className='save' onClick={isFavourite ? removeFavourite : addFavourite}>
                {isFavourite ? <i className="fa-solid fa-heart"></i> : <i className="fa-regular fa-heart"></i>}
            </button>)}
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
            {(userRole === "Customer" || userRole === "") && <button className='add-to-cart' onClick={addToCart}>Add to Cart</button>}
        </div>
    )
}

export default ProductCard