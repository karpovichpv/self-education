import React, { Component } from "react";
import ReactDom from "react-dom";
import { createRoot } from "react-dom/client";

class Person4 extends Component {
  static defaultProps = { name: "III", age: 22 };

  render() {
    return (
      <p>
        <b>Name</b>: {this.props.name} <b>Age</b>: {this.props.age}
      </p>
    );
  }
}

function Person(props) {
  return (
    <div>
      <p>Имя: {props.name}</p>
      <p>Возраст: {props.age}</p>
    </div>
  );
}

createRoot(document.getElementById("app")).render(
  <Person name="Tom" age="42"></Person>,
);

const personName = "Bob";
const personAge = 46;

createRoot(document.getElementById("app1")).render(
  <Person name={personName} age={personAge}></Person>,
);

function Person2({ user }) {
  return (
    <div>
      <p>Имя: {user.name}</p>
      <p>Возраст: {user.age}</p>
    </div>
  );
}

const john = { name: "John", age: 666 };

createRoot(document.getElementById("app2")).render(
  <Person2 user={john}></Person2>,
);

createRoot(document.getElementById("app3")).render(
  <Person name="Kimi" age={3333 - 333}></Person>,
);

function Person3(props) {
  return (
    <div>
      <h2>{props.say(props.name)}</h2>
    </div>
  );
}

function sayHello(name) {
  return `Hello, my name is ${name}`;
}

createRoot(document.getElementById("app4")).render(
  <Person3 name="Tom" say={sayHello} />,
);

const container = document.getElementById("app5");
const root = createRoot(container);

root.render(
  <div>
    <Person4 name="Bob" age="46" />
    <Person4 name="Bob" />
    <Person4 />
  </div>,
);
