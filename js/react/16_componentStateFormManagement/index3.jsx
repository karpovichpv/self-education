import { createRoot } from "react-dom/client";
import { useState, Component } from "react";
import React from "react";

function UserForm() {
  const [name, setName] = useState("");

  function onChange(e) {
    setName(e.target.value);
  }

  function handleSubmit(e) {
    e.preventDefault();
    console.log("Name: ", name);
  }

  return (
    <form onSubmit={handleSubmit}>
      <p>
        <label>Name:</label>
        <br />
        <input type="text" value={name} onChange={onChange} />
      </p>
      <input type="submit" value="Send"></input>
    </form>
  );
}

createRoot(document.getElementById("app3")).render(<UserForm></UserForm>);
