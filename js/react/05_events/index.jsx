import React from "react";
import ReactDom from "react-dom";
import { createRoot } from "react-dom/client";
import { Component } from "react";

function push() {
  console.log("button clicked!");
}

function push2(message) {
  console.log(message);
}

createRoot(document.getElementById("app0")).render(
  <button onClick={push}>Click</button>,
);

createRoot(document.getElementById("app1")).render(
  <button
    onClick={() => {
      console.log("button clicked2!");
    }}
  >
    Click2
  </button>,
);

createRoot(document.getElementById("app2")).render(
  <button
    onClick={() => {
      push2("button clicked3!");
    }}
  >
    Click3
  </button>,
);

function ClickButton(props) {
  function press() {
    console.log("Hello world!");
  }
  return <button onClick={press}>Click from a component</button>;
}

createRoot(document.getElementById("app3")).render(<ClickButton></ClickButton>);

class ClickButtonClass extends Component {
  constructor(props) {
    super(props);
    this.press = this.press.bind(this);
  }

  press() {
    console.log("Hello world from a class");
  }

  render() {
    return <button onClick={this.press}>Click from a class</button>;
  }
}

createRoot(document.getElementById("app4")).render(
  <ClickButtonClass></ClickButtonClass>,
);

class ClickButtonClass1 extends Component {
  press = () => {
    console.log("Hello world from the class2");
  };

  render() {
    return <button onClick={this.press}>Click from a class 2</button>;
  }
}

createRoot(document.getElementById("app5")).render(
  <ClickButtonClass1></ClickButtonClass1>,
);

function ClickButtonEvent(props) {
  function press(e) {
    console.log(e);
    console.log("Hello world");
  }

  return <button onClick={press}>Click Event</button>;
}

createRoot(document.getElementById("app6")).render(
  <ClickButtonEvent></ClickButtonEvent>,
);

class ClickButtonEventClass extends Component {
  constructor(props) {
    super(props);
    this.press = this.press.bind(this);
  }
  press(e) {
    console.log(e);
    console.log("Hello world from the event class");
  }
  render() {
    return <button onClick={this.press}>Click from event class</button>;
  }
}

createRoot(document.getElementById("app7")).render(
  <ClickButtonEventClass></ClickButtonEventClass>,
);
