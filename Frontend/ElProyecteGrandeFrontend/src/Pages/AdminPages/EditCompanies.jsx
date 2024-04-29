import { useState, useEffect } from 'react';
import './Admin.css';
import '../../index.css'
import { Button, Modal } from 'flowbite-react';



function EditCompanies() {
    const [users, setUsers] = useState([])
    const [userRole, setUserRole] = useState("")
    const [roles, setRoles] = useState([])
    const [showDetails, setShowDetails] = useState(false)
    const [showDelete, setShowDelete] = useState(false)
    const [currentUser, setCurrentUser] = useState(null)
    const [showVerify, setShowVerify] = useState(false)

    async function fetchUsers() {
        let url = `http://localhost:5036/User/GetAllCompanies`;
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
        fetchUsers()
            .then(users => setUsers(users))
    }, [])

    async function handleDeleteUser(user) {
        let url = `http://localhost:5036/User/DeleteUserForAdmin?id=${user.id}`;
        const res = await fetch(url,
            {
                method: "DELETE",
                credentials: 'include'
            });

    }

    async function handleVerifyCompany(user) {
        let url = `http://localhost:5036/User/VerifyCompany?userName=${user.userName}&verified=true`;
        const res = await fetch(url,
            {
                method: "PATCH",
                credentials: 'include'
            });

    }

    return (
        <>
            <div className="admin-page">
                {users.length !== 0 ?
                    <table>
                        <thead>
                            <tr>
                                <th>User name</th>
                                <th>Email</th>
                                <th>Role</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map((user) =>
                                <tr key={user.userName}>
                                    <td>{user.userName}</td>
                                    <td>{user.email}</td>
                                    <td>role</td>
                                    <td>
                                        <button className="adminButton" id="details" onClick={((e) => { setShowDetails(true), setCurrentUser(user) })}>Details</button>
                                        <button className="adminButton" id="delete" onClick={(e => { setShowDelete(true), setCurrentUser(user) })}>Delete</button>
                                        <button className="adminButton" disabled={user.company.verified === true ? true : false} id="Verify" onClick={(e => { setShowVerify(true), setCurrentUser(user) })}>Verify</button>
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
                        <h1>{currentUser !== null ? currentUser.userName : ""}</h1>
                        <button className='close-modal' onClick={e => setShowDetails(false)}>X</button>
                    </div>
                    <div>
                        <table className='user-details-table'>
                            <tbody className='user-details-tbody'>
                                <tr className='user-details-tr'>
                                    <td>User name</td>
                                    <td>{currentUser !== null ? currentUser.userName : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Company name</td>
                                    <td>{currentUser !== null ? currentUser.company.name : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Id</td>
                                    <td>{currentUser !== null ? currentUser.id : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Email</td>
                                    <td>{currentUser !== null ? currentUser.email : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Phone</td>
                                    <td>{currentUser !== null ? (currentUser.phoneNumber !== null ? currentUser.phoneNumber : "no phone number") : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Products</td>
                                    <td>{currentUser !== null ? (currentUser.companyProducts !== 0
                                        ? currentUser.companyProducts.map(product => <div><p><span style={{ fontWeight: '400' }}>
                                            Id: </span> {product.id} | <span style={{ fontWeight: '400' }}>
                                                Name: </span> {product.name} | <span style={{ fontWeight: '400' }}>
                                                Quantity: </span> {product.quantity}</p><p></p></div>) :
                                        "no products yet") : ""}</td>
                                </tr>
                                <tr className='user-details-tr'>
                                    <td>Verified</td>
                                    <td>{currentUser !== null ? (currentUser.company.verified === false ? "false" : "true") : ""}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </Modal.Body>
            </Modal >
            <Modal className='details-modal' dismissible show={showDelete} onClose={() => setShowDelete(false)}>
                <Modal.Body className='details-modal-body'>
                    <div className='modal-header'>
                        <h1>Delete user: {currentUser !== null ? currentUser.userName : ""}</h1>
                        <button className='close-modal' onClick={e => setShowDelete(false)}>X</button>
                    </div>
                    <div>
                        <h3>Are you sure you want to delete this user?</h3>
                    </div>
                    <div className='modal-header'>
                        <button className='delete-user' onClick={e => { handleDeleteUser(currentUser), setShowDelete(false) }}>Yes</button>
                        <button className='close-modal' onClick={e => setShowDelete(false)}>No</button>
                    </div>
                </Modal.Body>
            </Modal>
            <Modal className='details-modal' dismissible show={showVerify} onClose={() => setShowVerify(false)}>
                <Modal.Body className='details-modal-body'>
                    <div className='modal-header'>
                        <h1>Verify company: {currentUser !== null ? currentUser.company.name : ""}</h1>
                        <button className='close-modal' onClick={e => setShowVerify(false)}>X</button>
                    </div>
                    <div>
                        <h3>Are you sure you want to verify this company?</h3>
                    </div>
                    <div className='modal-header'>
                        <button className='delete-user' onClick={e => { handleVerifyCompany(currentUser), setShowVerify(false) }}>Yes</button>
                        <button className='close-modal' onClick={e => setShowVerify(false)}>No</button>
                    </div>
                </Modal.Body>
            </Modal>
        </>
    )
}

export default EditCompanies;