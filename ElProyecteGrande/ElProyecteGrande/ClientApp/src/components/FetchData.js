import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { user: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(user) {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Username</th>
            <th>password</th>
          </tr>
        </thead>
        <tbody>
        {user.map(user => 
          <tr key={user.id}>
            <th>{user.username}</th>
            <th>{user.password}</th>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.user);

    return (
      <div>
        <h1 id="tableLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('User/GetAllUser');
    const data = await response.json();
    console.log(data)
    console.log(data[0])
    this.setState({ user: data, loading: false });
  }
}
