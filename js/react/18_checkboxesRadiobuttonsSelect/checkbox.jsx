import { createRoot } from "react-dom/client";
import { useState } from "react";
import React from "react";
import { Component } from "react";

function Terms() {
  const [isChecked, setIsChecked] = useState(false);
  const handleChange = () => {
    setIsChecked((prev) => !prev);
  };

  return (
    <>
      <label>Accept rules:</label>
      <input
        type="checkbox"
        checked={isChecked}
        onChange={handleChange}
      ></input>
      <p>{isChecked ? "Rules accepted" : "You need to accept rules"}</p>
    </>
  );
}

createRoot(document.getElementById("checkbox")).render(<Terms></Terms>);

class TermsClass extends Component {
  constructor(props) {
    super(props);
    this.state = { isChecked: false };
  }
  handleChange = () => {
    this.setState({ isChecked: !this.state.isChecked });
  };

  render() {
    return (
      <>
        <label>Accept rules:</label>
        <input
          type="checkbox"
          checked={this.state.isChecked}
          onChange={this.handleChange}
        ></input>
        <p>
          {this.state.isChecked ? "Rules accepted" : "You need to accept rules"}
        </p>
      </>
    );
  }
}

createRoot(document.getElementById("checkboxClass")).render(
  <TermsClass></TermsClass>,
);
