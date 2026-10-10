import { createRoot } from "react-dom/client";
import { useState, Component } from "react";
import React from "react";

class UserForm extends Component {
  constructor(props) {
    super(props);
    this.state = { name: "" };
    this.onChange = this.onChange.bind(this);
  }

  onChange(e) {
    this.setState({ name: e.target.value });
  }

  render() {
    return (
      <div>
        <input
          type="text"
          value={this.state.name}
          onChange={this.onChange}
        ></input>
        <p>Name: {this.state.name}</p>
      </div>
    );
  }
}

createRoot(document.getElementById("app2")).render(<UserForm></UserForm>);
