import React from "react";
import { Component } from "react";
import { createRoot } from "react-dom/client";

const langs = {
  title: "Programming langs",
  items: ["js", "c++", "ts", "java", "c#", "python"],
};

function Item(props) {
  return <li>{props.name}</li>;
}

function ItemsList(props) {
  return (
    <div>
      <h2>{props.data.title}</h2>
      <ul>
        {props.data.items.map((item) => (
          <Item key={item} name={item}></Item>
        ))}
      </ul>
    </div>
  );
}

createRoot(document.getElementById("app")).render(
  <ItemsList data={langs}></ItemsList>,
);

class ItemClass extends Component {
  render() {
    return <li>{this.props.name}</li>;
  }
}

class ItemListClass extends Component {
  render() {
    return (
      <div>
        <h2>{this.props.data.title}</h2>
        <ul>
          {this.props.data.items.map((item) => (
            <Item key={item} name={item}></Item>
          ))}
        </ul>
      </div>
    );
  }
}

createRoot(document.getElementById("app1")).render(
  <ItemsList data={langs}></ItemsList>,
);
