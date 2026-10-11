import { createRoot } from "react-dom/client";
import { useState } from "react";
import React from "react";
import { Component } from "react";

function Language() {
  const [selectedOption, setSelectedOption] = useState("");
  const handleChange = (e) => setSelectedOption(e.target.value);

  return (
    <div>
      <select value={selectedOption} onChange={handleChange}>
        <option value="js">Choose a language</option>
        <option value="js">js</option>
        <option value="ts">ts</option>
        <option value="python">python</option>
      </select>
      <p>
        Selected Language:{" "}
        {selectedOption === "" ? "Not selected" : selectedOption}
      </p>
    </div>
  );
}

createRoot(document.getElementById("select")).render(<Language></Language>);

class LanguageClass extends Component {
  constructor(props) {
    super(props);
    this.state = { selectedOption: "" };
  }
  handleChange = (e) => this.setState({ selectedOption: e.target.value });

  render() {
    return (
      <div>
        <select value={this.state.selectedOption} onChange={this.handleChange}>
          <option value="js">Choose a language</option>
          <option value="js">js</option>
          <option value="ts">ts</option>
          <option value="python">python</option>
        </select>
        <p>
          Selected Language:{" "}
          {this.state.selectedOption === ""
            ? "Not selected"
            : this.state.selectedOption}
        </p>
      </div>
    );
  }
}

createRoot(document.getElementById("selectClass")).render(
  <Language></Language>,
);
