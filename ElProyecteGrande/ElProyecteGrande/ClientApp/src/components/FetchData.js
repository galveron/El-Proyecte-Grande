import React, { useState, useEffect } from 'react';

function FetchData(){
  
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  
   useEffect(() => {
    let data = fetchAllUser();
    data.then(userData => setUsers(userData), setLoading(false))
  },[])
  
  async function fetchAllUser(){
     try {
       const response = await fetch('User/GetAllUser');
       return await response.json();
     } catch (error){
       throw error
     }
  }
  
  function RenderUsersTable() {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Username</th>
            <th>password</th>
          </tr>
        </thead>
        <tbody>
        {users.map(user => 
          <tr key={user.id}>
            <th>{user.username}</th>
            <th>{user.password}</th>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

    return (
      <div>
        <h1 id="tableLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {loading ? <p><em>Loading...</em></p> : RenderUsersTable()}
      </div>
    );

}

export default FetchData;
