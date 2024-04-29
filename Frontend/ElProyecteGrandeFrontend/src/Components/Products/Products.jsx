import ProductCard from "../ProductCard/ProductCard";
import React from "react";
import './Products.css'

function Products({products, user, handleSetUser, userRole, setCustomer}) {

    return (
        <>
            <article className="products">
                <div className="category">
                    {products ?
                        products.map((product) => <ProductCard {...{product, user, handleSetUser, userRole, setCustomer}} key={product.id}/>)
                        : <h2>Loading...</h2>}
                </div>
            </article>
        </>
    )
}

export default Products