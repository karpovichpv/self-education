import React from "react";

import { createRoot } from "react-dom/client";

function Hello() {
  return <h1>Hello world</h1>;
}

createRoot(document.getElementById("app")).render(<Hello></Hello>);
