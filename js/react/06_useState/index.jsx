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

function User1() {
  const [name, setName] = useState("Tom");
  const [age, setAge] = useState(42);

  return (
    <div>
      <h3>Имя: {name}</h3>
      <h3>Возраст: {age}</h3>
    </div>
  );
}

createRoot(document.getElementById("app1")).render(<User1></User1>);

function User2() {
  const [name, setName] = useState("Tom");
  const [age, setAge] = useState(53);

  function handleNameChange(event) {
    setName(event.target.value);
  }

  function handleAgeChange(event) {
    setAge(event.target.value);
  }

  return (
    <div>
      <h3>Имя: {name}</h3>
      <h3>Возраст: {age}</h3>
      <div>
        <p>
          Имя:{" "}
          <input type="text" value={name} onChange={handleNameChange}></input>
        </p>
        <p>
          Возраст:{" "}
          <input
            type="number"
            min="0"
            max="110"
            value={age}
            onChange={handleAgeChange}
          ></input>
        </p>
      </div>
    </div>
  );
}

createRoot(document.getElementById("app2")).render(<User2></User2>);

function User3() {
  const [newUser, setUser] = useState({ name: "John", age: 55 });

  function handleNameChange(event) {
    setUser({ name: event.target.value, age: newUser.age });
  }

  function handleAgeChange(event) {
    setUser({ name: newUser.name, age: event.target.value });
  }

  return (
    <div>
      <h3>Name: {newUser.name}</h3>
      <h3>Age: {newUser.age}</h3>
      <div>
        Name:{" "}
        <input
          type="text"
          value={newUser.name}
          onChange={handleNameChange}
        ></input>
      </div>
      <div>
        Age:{" "}
        <input
          type="number"
          min="0"
          max="110"
          value={newUser.age}
          onChange={handleAgeChange}
        ></input>
      </div>
    </div>
  );
}

createRoot(document.getElementById("app3")).render(<User3 />);

function User4() {
  const [user, setUser] = useState({ name: "Vic", age: "773" });

  function handelNameChange(event) {
    setUser({ ...user, name: event.target.value });
  }

  function handelAgeChange(event) {
    setUser({ ...user, age: event.target.value });
  }
  return (
    <div>
      <h3>Name: {user.name}</h3>
      <h3>Age: {user.age}</h3>
      <div>
        Set Name:
        <input
          type="text"
          value={user.name}
          onChange={handelNameChange}
        ></input>
      </div>
      <div>
        Set age:{" "}
        <input
          type="number"
          min="10"
          max="200"
          value={user.age}
          onChange={handelAgeChange}
        ></input>
      </div>
    </div>
  );
}

createRoot(document.getElementById("app4")).render(<User4></User4>);
