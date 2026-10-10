import React from "react";
import { createRoot } from "react-dom/client";
import { Component } from "react";

class User1 extends Component {
  constructor(props) {
    super(props);
    this.state = { name: "Tom" };
    this.setSharedState = this.setSharedState.bind(this);
  }

  setSharedState(value) {
    this.setState({ name: value });
  }
  render() {
    return (
      <>
        <Message state={this.state} />
        <UserEdit state={this.state} setState={this.setSharedState} />
      </>
    );
  }
}

class Message extends Component {
  constructor(props) {
    super(props);
  }
  render() {
    return (
      <div>
        <h1>Hello {this.props.state.name}</h1>
      </div>
    );
  }
}

class UserEdit extends Component {
  /**
   *
   */
  constructor(props) {
    super(props);
    this.onStateChanged = this.onStateChanged.bind(this);
  }

  onStateChanged(e) {
    this.props.setState(e.target.value);
  }
  render() {
    return (
      <div>
        <input
          type="text"
          value={this.props.state.name}
          onChange={this.onStateChanged}
        />
        <h3>Current name: {this.props.state.name}</h3>
      </div>
    );
  }
}

createRoot(document.getElementById("app")).render(<User1></User1>);
