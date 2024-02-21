import ProductCard from "./ProductCard";
import React from "react";

function Products(props) {
    const { products } = props
    return (
        <>
            <article className="products">
                <div className="category">
                    {products.length > 0 ?
                        products.map((product) => <ProductCard product={product} />)
                        : <h2>Loading...</h2>}
                </div>
            </article>
        </>
    )
}

export default Products