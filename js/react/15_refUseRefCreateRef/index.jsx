import React from "react";
import { createRoot } from "react-dom/client";
import { useRef } from "react";

function UserForm() {
  const nameField = useRef(null);
  const send = () => {
    const inputElement = nameField.current;
    console.log("Name: " + inputElement.value);
  };

  return (
    <div>
      <input ref={nameField} />
      <button onClick={send}>Send</button>
    </div>
  );
}

createRoot(document.getElementById("app")).render(<UserForm />);
