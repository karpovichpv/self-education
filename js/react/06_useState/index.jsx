import React from "react";
import ReactDom from "react-dom";
import { createRoot } from "react-dom/client";
import { Component } from "react";
import { useState } from "react";

function Counter() {
  const [count, setCount] = useState(0);
  return (
    <div>
      <h3>Count = {count}</h3>
      <button onClick={() => setCount(count + 1)}>Click</button>
    </div>
  );
}

createRoot(document.getElementById("app")).render(<Counter></Counter>);
