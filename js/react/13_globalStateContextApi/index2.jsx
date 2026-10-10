import React from "react";
import { useState, createContext, useContext } from "react";
import { createRoot } from "react-dom/client";
import { Component } from "react";

const AuthContext = createContext();

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  return (
    <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated }}>
      <h1>{isAuthenticated ? "Authenticated" : "No Access"}</h1>
      <Login />
    </AuthContext.Provider>
  );
}

function Login() {
  const { isAuthenticated, setIsAuthenticated } = useContext(AuthContext);

  return (
    <button onClick={() => setIsAuthenticated(!isAuthenticated)}>
      {isAuthenticated ? "Logout" : "Login"}
    </button>
  );
}

createRoot(document.getElementById("app1")).render(<App></App>);
