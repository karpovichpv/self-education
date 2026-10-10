import { Component } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { createRoot } from "react-dom/client";
import React from "react";

class ClickButton extends Component {
  constructor(props) {
    super(props);
    this.state = { class: "off", label: "Push" };

    this.press = this.press.bind(this);
    console.log("constructor");
  }

  static getDerivedStateFromProps(props, state) {
    console.log("getDerivedStateFromProps()");
    return null;
  }

  componentDidMount() {
    console.log("componentDidMount()");
  }
  componentWillUnmount() {
    console.log("componentWillUnmount()");
  }
  shouldComponentUpdate() {
    console.log("shouldComponentUpdate()");
    return true;
  }

  getShapshotBeforeUpdate(prevProps, prevState) {
    console.log("getSnapshotBeforeUpdate()");
  }

  componentDidUpdate() {
    console.log("componentDidUpdate()");
  }

  press() {
    let className = (this.state.class = "off") ? "on" : "off";
    this.setState({ class: className });
    console.log("press()");
  }

  render() {
    console.log("render()");
    return (
      <button onClick={this.press} className={this.state.class}>
        {this.state.label}
      </button>
    );
  }
}

createRoot(document.getElementById("app")).render(<ClickButton></ClickButton>);

function ClickButton1(props) {
  const [buttonClass, setButtonClass] = useState("off");
  const [didMount, setDidMount] = useState(false);

  if (!didMount) {
    console.log("constructor");
  }

  useEffect(() => {
    console.log("componentDidMount");
    setDidMount(true);

    return () => {
      console.log("componentWillUnmount");
    };
  }, []);

  useEffect(() => {
    console.log("componentDidUpdate");
  });

  function press() {
    const className = buttonClass === "off" ? "on" : "off";
    setButtonClass(className);
  }

  return (
    <>
      {console.log("render()")};
      <button onClick={press} className={buttonClass}>
        Press
      </button>
    </>
  );
}

createRoot(document.getElementById("app1")).render(<ClickButton1 />);
