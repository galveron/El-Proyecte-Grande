import { useState, useEffect } from 'react';
import './Admin.css';
import '../../index.css'



function EditUsers() {
    const [users, setUsers] = useState([])
    const [userRole, setUserRole] = useState("")
    const [roles, setRoles] = useState([])
    const [showDetails, setShowDetails] = useState(false)
    const [showDelete, setShowDelete] = useState(false)

    async function fetchUsers() {
        let url = `http://localhost:5036/User/GetAllUsers`;
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
                            {users.map((user, i) =>
                                <tr key={user.userName}>
                                    <td>{user.userName}</td>
                                    <td>{user.email}</td>
                                    <td>role</td>
                                    <td>
                                        <button class="adminButton" id="details" onClick={(e => setShowDetails(true))}>Details</button>
                                        <button class="adminButton" id="delete" onClick={(e => setShowDelete(true))}>Delete</button>
                                    </td>
                                </tr>
                            )}

                        </tbody>
                    </table>
                    : <p>no users</p>}
            </div >
            {
                showDetails ?
                    <div className='details-modal'>
                        Hello
                    </div>
                    : <div>By</div>
            }
            {
                showDelete ?
                    <div className='delete-modal'>
                        Hello
                    </div>
                    : <div>By</div>
            }
        </>
    )
}

export default EditUsers;