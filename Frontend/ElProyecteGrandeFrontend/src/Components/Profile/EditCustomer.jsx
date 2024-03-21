import { useState } from "react";
import { notification } from 'antd';
import { Modal } from 'flowbite-react';

notification.config({
    duration: 2,
    closeIcon: null
})


function EditCustomer({ user, showEdit, setShowEdit, setUser }) {
    const [email, setEmail] = useState(user.email);
    const [username, setUsername] = useState(user.userName);
    const [phoneNum, setPhoneNum] = useState(user.phoneNumber);

    async function fetchUser() {
        if (user) {
            let url = `http://localhost:5036/User/GetUser`;
            const res = await fetch(url,
                {
                    method: "GET",
                    credentials: 'include'
                });
            console.log("fetchuser");
            console.log(res);
            const data = await res.json();
            return data;
        }
    }

    async function handleSave(e) {
        e.preventDefault();
        try {
            const res = await fetch(`http://localhost:5036/User/UpdateCustomer?userName=${username}&email=${email}&phoneNumber=${phoneNum}`, {
                method: 'PATCH',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' }
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            notification.success({ message: 'Profile info updated.' });
            const userAfterSave = await fetchUser();
            setUser(userAfterSave);
        }
        catch (error) {
            throw error;
        }
    }

    return (
        <Modal className='details-modal' dismissible show={showEdit} onClose={() => setShowEdit(false)}>
            <Modal.Body className='details-modal-body'>
                <div className='modal-header'>
                    <h1>Edit</h1>
                    <button className='close-modal' onClick={e => setShowEdit(false)}>X</button>
                </div>
                <div>
                    <table className='user-details-table'>
                        <tbody className='user-details-tbody'>
                            <tr className='user-details-tr'>
                                <td>Username</td>
                                <td>{user !== null ? user.userName : ""}</td>
                            </tr>
                            <tr className='user-details-tr'>
                                <td>Email</td>
                                <td>
                                    <input type="text" onChange={e => setEmail(e.target.value)} value={email}></input>
                                </td>
                            </tr>
                            <tr className='user-details-tr'>
                                <td>Phone</td>
                                <td>
                                    <input type="text" onChange={e => setPhoneNum(e.target.value)} value={phoneNum}></input>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <button onClick={e => handleSave(e)}>Save</button>
                </div>
            </Modal.Body>
        </Modal>
    )
}

export default EditCustomer;