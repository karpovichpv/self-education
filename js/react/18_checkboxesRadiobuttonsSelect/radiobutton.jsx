import { createRoot } from "react-dom/client";
import { useState } from "react";
import React from "react";
import { Component } from "react";

function Language() {
  const [selectedOption, setSelectedOption] = useState("");
  const handleChange = (e) => setSelectedOption(e.target.value);

  return (
    <div>
      <input
        type="radio"
        value="js"
        checked={selectedOption === "js"}
        onChange={handleChange}
      />
      js
      <br />
      <input
        type="radio"
        value="ts"
        checked={selectedOption === "ts"}
        onChange={handleChange}
      />
      ts
      <br />
      <input
        type="radio"
        value="python"
        checked={selectedOption === "python"}
        onChange={handleChange}
      />
      python
      <br />
      <p>Selected Option: {selectedOption}</p>
    </div>
  );
}

createRoot(document.getElementById("radiobutton")).render(
  <Language></Language>,
);

class LanguageClass extends Component {
  constructor(props) {
    super(props);
    this.state = { selectedOption: "" };
  }
  handleChange = (e) => this.setState({ selectedOption: e.target.value });

  render() {
    return (
      <div>
        <input
          type="radio"
          value="js"
          checked={this.state.selectedOption === "js"}
          onChange={this.handleChange}
        />
        js
        <br />
        <input
          type="radio"
          value="ts"
          checked={this.state.selectedOption === "ts"}
          onChange={this.handleChange}
        />
        ts
        <br />
        <input
          type="radio"
          value="python"
          checked={this.state.selectedOption === "python"}
          onChange={this.handleChange}
        />
        python
        <br />
        <p>Selected Option: {this.state.selectedOption}</p>
      </div>
    );
  }
}

createRoot(document.getElementById("radiobuttonClass")).render(
  <LanguageClass></LanguageClass>,
);
