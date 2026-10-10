import { createRoot } from "react-dom/client";
import { Component } from "react";
import React from "react";

class UserForm extends Component {
  constructor(props) {
    super(props);
    this.state = { username: "Undefined", userage: 18 };
  }

  handleChange = (e) => {
    const { name, value } = e.target;
    this.setState((prevData) => ({ ...prevData, [name]: value }));
  };

  render() {
    return (
      <div>
        <p>
          Name: {this.state.username} <br /> Age: {this.state.userage}
        </p>

        <p>
          <label>Name</label>
          <br />
          <input
            type="text"
            name="username"
            value={this.state.username}
            onChange={this.handleChange}
          ></input>
        </p>

        <p>
          <label>Age</label>
          <br />
          <input
            type="number"
            name="userage"
            value={this.state.userage}
            onChange={this.handleChange}
          ></input>
        </p>
      </div>
    );
  }
}

createRoot(document.getElementById("app")).render(<UserForm></UserForm>);
