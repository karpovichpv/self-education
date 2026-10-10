import React from "react";
import { createRoot } from "react-dom/client";
import { useState } from "react";

function UserForm() {
  const [name, setName] = useState("");

  function onChange(e) {
    setName(e.target.value);
  }

  return (
    <div>
      <input type="text" value={name} onChange={onChange} />
      <p>Name: {name}</p>
    </div>
  );
}

createRoot(document.getElementById("app")).render(<UserForm></UserForm>);
