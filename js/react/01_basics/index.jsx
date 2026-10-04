import React from "react";
import ReactDOM from "react-dom/client";
import { createRoot } from "react-dom/client";

const user = {
  id: 44,
  age: 33,
  firstName: "Tom",
  lastName: "Smit",
  getFullName: function () {
    return `${this.firstName} ${this.lastName}`;
  },
};

const userClassName = "user-info";
const styleObj = {
  color: "navy",
  fontFamily: "Verdana",
};

const langs = ["JS", "TS", "Java", "C#", "Python"];

let isLoggedIn = true;

createRoot(document.getElementById("app")).render(
  <div id={user.id} className="{userClassName}" style={styleObj}>
    <p>Полное имя: {user.getFullName()}</p>
    <p>Возраст: {user.age}</p>
    <p>Время генерации данных: {new Date().toLocaleDateString()}</p>

    <ul>
      {langs.map((lang, index) => (
        <li key={index}>{lang}</li>
      ))}
    </ul>

    {isLoggedIn ? (
      <h1>Добро пожаловать</h1>
    ) : (
      <h1>Необходимо выполнить авторизацию</h1>
    )}
  </div>,
);
