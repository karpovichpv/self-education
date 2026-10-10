import React from "react";
import { useState, createContext, useContext } from "react";
import { createRoot } from "react-dom/client";
import { Component } from "react";

const UserContext = createContext();

function User() {
  const [user, setUser] = useState({ name: "Tom", age: 40 });

  return (
    <UserContext.Provider value={user}>
      <UserProfile></UserProfile>
    </UserContext.Provider>
  );
}

function UserProfile() {
  const user = useContext(UserContext);
  return <p> Name: {user.name}</p>;
}

createRoot(document.getElementById("app")).render(<User></User>);
