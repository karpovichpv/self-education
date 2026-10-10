import React, { useEffect } from "react";
import { createRoot } from "react-dom/client";
import { useRef } from "react";
import { useState } from "react";
import { Component, createRef } from "react";

class UserForm5 extends Component {
  constructor(props) {
    super(props);
    this.nameField = createRef();
  }

  send = () => {
    const inputElement = this.nameField.current;
    console.log("Name: ", inputElement.value);
  };
  render() {
    return (
      <div>
        <input defaultValue="Tom" ref={this.nameField} />
        <button onClick={this.send}>Send</button>
      </div>
    );
  }
}

createRoot(document.getElementById("app2")).render(<UserForm5></UserForm5>);
