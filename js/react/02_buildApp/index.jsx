import React from "react";
import ReactDom from "react-dom/client";
const rootNode = document.getElementById("app");

const root = ReactDom.createRoot(rootNode);
const element = React.createElement("h1", null, "Hello World111!");

root.render(element);
