import ProductCard from "../ProductCard/ProductCard";
import React from "react";
import './Products.css'

function Products({products, user}) {
    return (
        <>
            <article className="products">
                <div className="category">
                    {products.length > 0 ?
                        products.map((product) => <ProductCard product={product} key={product.id} user={user} />)
                        : <h2>Loading...</h2>}
                </div>
            </article>
        </>
    )
}

export default Products