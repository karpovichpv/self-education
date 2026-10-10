import React from "react";
import { useState, useEffect } from "react";
import { createRoot } from "react-dom/client";
import { Component } from "react";

function User() {
  const [name, setName] = useState("Tom");

  useEffect(() => {
    document.title = `Привет ${name}`;
  });

  function changeName(event) {
    setName(event.target.value);
  }

  return (
    <div>
      <h3>Name: {name}</h3>
      <p>
        Имя: <input type="text" value={name} onChange={changeName} />
      </p>
    </div>
  );
}

//createRoot(document.getElementById("app")).render(<User />);

function User2() {
  const [name, setName] = useState("Tom");
  const [age, setAge] = useState(33);

  useEffect(() => {
    document.title = `Привет ${name}`;
    console.log("useEffect");
  }, [name]);

  function changeName(event) {
    setName(event.target.value);
  }

  function changeAge(event) {
    setAge(event.target.value);
  }

  return (
    <div>
      <h3>Name: {name}</h3>
      <div>
        <p>
          Name: <input type="text" value={name} onChange={changeName} />
        </p>
        <p>
          Age: <input type="number" value={age} onChange={changeAge} />
        </p>
      </div>
    </div>
  );
}

//createRoot(document.getElementById("app2")).render(<User2 />);

function User3() {
  const [name, setName] = useState("Tom");

  const unmount = () => root.unmount();
  useEffect(() => {
    const unmountBtn = document.getElementById("unmountBtn");
    // подписываемся на событие onclick кнопки unmountBtn
    unmountBtn.addEventListener("click", unmount);
    console.log("EventListener added");

    return () => {
      // отписываемся от события
      unmountBtn.removeEventListener("click", unmount);
      console.log("EventListener removed");
    };
  }, []); // эффект срабатывает только один раз - при самом первом рендеринге

  return (
    <div>
      <h3>Имя: {name}</h3>
      <p>
        Имя:{" "}
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </p>
    </div>
  );
}

createRoot(document.getElementById("app3")).render(<User3 />);
