import React, { useEffect } from "react";
import { createRoot } from "react-dom/client";
import { useRef } from "react";
import { useState } from "react";

const root = createRoot(document.getElementById("app1"));

function UserForm() {
  const [name, setName] = useState("Tom");
  const nameRef = useRef(name);

  useEffect(() => {
    nameRef.current = name;
  }, [name]);

  useEffect(() => {
    const userName = localStorage.getItem("userName");

    if (userName != null) {
      setName(userName);
      console.log("Got!");
    }

    return () => {
      console.log(nameRef.current);
      localStorage.setItem("userName", nameRef.current);
      console.log("saved!");
    };
  }, []);

  const changeName = (event) => setName(event.target.value);
  const unmount = () => root.unmount();

  return (
    <div>
      <h3>Name: {name}</h3>
      <div>
        <p>
          Name: <input value={name} onChange={changeName}></input>
        </p>
        <button onClick={unmount}>Unmount</button>
      </div>
    </div>
  );
}

root.render(<UserForm></UserForm>);
