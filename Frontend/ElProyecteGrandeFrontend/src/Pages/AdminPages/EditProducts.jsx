import { useState, useEffect } from 'react';
import './Admin.css';
import '../../index.css'
import { Button, Modal } from 'flowbite-react';



function EditProducts() {
    const [products, setProducts] = useState([])
    const [userRole, setUserRole] = useState("")
    const [showDetails, setShowDetails] = useState(false)
    const [showDelete, setShowDelete] = useState(false)
    const [currentProduct, setCurrentProduct] = useState(null)
    const [onlyUsers, setOnlyUsers] = useState([])

    async function fetchProducts() {
        let url = `http://localhost:5036/Product/GetAllProducts`;
        const res = await fetch(url,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        const data = await res.json();
        return data;
    }

    useEffect(() => {
        fetchProducts()
            .then(product => setProducts(product))
    }, [])

    async function handleDeleteProduct(product) {
        let url = `http://localhost:5036/Product/DeleteProduct?id=${product.id}`;
        const res = await fetch(url,
            {
                method: "DELETE",
                credentials: 'include'
            });
    }

    return (
        <>
            <div className="admin-page">
                {products.length !== 0 ?
                    <table>
                        <thead>
                            <tr>
                                <th>Product name</th>
                                <th>Price</th>
                                <th>Quantity</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {products.map((product) =>
                                <tr key={product.id}>
                                    <td>{product.name}</td>
                                    <td>{product.price} Ft</td>
                                    <td>{product.quantity} db</td>
                                    <td>
                                        <button className="adminButton" id="details" onClick={((e) => { setShowDetails(true), setCurrentProduct(product) })}>Details</button>
                                        <button className="adminButton" id="delete" onClick={(e => { setShowDelete(true), setCurrentProduct(product) })}>Delete</button>
                                    </td>
                                </tr>
                            )}

                        </tbody>
                    </table>
                    : <p>no users</p>}
            </div >
            <Modal className='details-modal' dismissible show={showDetails} onClose={() => setShowDetails(false)}>
                <Modal.Body className='details-modal-body'>
                    <div className='modal-header'>
                        <h1>{currentProduct !== null ? currentProduct.name : ""}</h1>
                        <button className='close-modal' onClick={e => setShowDetails(false)}>X</button>
                    </div>
                    <div>
                        IMAGES
                    </div>
                    <div>
                        <table className='user-details-table'>
                            <tbody className='user-details-tbody'>
                                <tr className='user-details-tr'>
                                    <td>Name</td>
                                    <td>{currentProduct !== null ? currentProduct.name : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Id</td>
                                    <td>{currentProduct !== null ? currentProduct.id : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Price</td>
                                    <td>{currentProduct !== null ? currentProduct.price : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Quantity</td>
                                    <td>{currentProduct !== null ? currentProduct.quantity : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Details</td>
                                    <td>{currentProduct !== null ? currentProduct.details : ""}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </Modal.Body>
            </Modal>
            <Modal className='details-modal' dismissible show={showDelete} onClose={() => setShowDelete(false)}>
                <Modal.Body className='details-modal-body'>
                    <div className='modal-header'>
                        <h1>Delete product: {currentProduct !== null ? currentProduct.name : ""}</h1>
                        <button className='close-modal' onClick={e => setShowDelete(false)}>X</button>
                    </div>
                    <div>
                        <h3>Are you sure you want to delete this product?</h3>
                    </div>
                    <div className='modal-header'>
                        <button className='delete-user' onClick={e => { handleDeleteProduct(currentProduct), setShowDelete(false) }}>Yes</button>
                        <button className='close-modal' onClick={e => setShowDelete(false)}>No</button>
                    </div>
                </Modal.Body>
            </Modal>
        </>
    )
}

export default EditProducts;