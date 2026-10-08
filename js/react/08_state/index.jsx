import React from "react";
import { Component } from "react";
import { createRoot } from "react-dom/client";
import { useState } from "react";

class Hello extends Component {
  constructor(props) {
    super(props);
    this.state = { message: "Hello world!" };
  }
  render() {
    return <h1>{this.state.message}</h1>;
  }
}

createRoot(document.getElementById("app")).render(<Hello></Hello>);

class Hello1 extends Component {
  constructor(props) {
    super(props);
    this.state = { name: "Tom", age: "53" };
  }
  render() {
    return (
      <div>
        <h1>Name: {this.state.name}</h1>
        <h1>Age: {this.state.age}</h1>
      </div>
    );
  }
}

createRoot(document.getElementById("app1")).render(<Hello1></Hello1>);

class Hello2 extends Component {
  constructor(props) {
    super(props);
    this.state = { message: "Hello world3!" };
    this.messageChange = this.messageChange.bind(this);
  }

  messageChange(e) {
    this.setState({ message: event.target.value });
  }

  render() {
    return (
      <div>
        <h2>{this.state.message}</h2>
        <input
          type="text"
          value={this.state.message}
          onChange={this.messageChange}
        ></input>
      </div>
    );
  }
}

createRoot(document.getElementById("app2")).render(<Hello2 />);
