import { createRoot } from "react-dom/client";
import { useState } from "react";
import React from "react";

function EmailForm() {
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    setEmail(e.target.value);

    if (!emailRegex.test(email)) {
      setError("Invalid email address");
    } else setError("");
  };

  return (
    <div>
      <input
        type="email"
        value={email}
        onChange={handleChange}
        placeholder="Email"
      />
      {error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : (
        <p style={{ color: "green" }}>{email}</p>
      )}
    </div>
  );
}

createRoot(document.getElementById("emailForm")).render(
  <EmailForm></EmailForm>,
);

function UserForm(props) {
  const initialName = props.name || "";
  const initialAge = props.age !== undefined ? props.age : "";

  const validateAge = (age) => age >= 0;
  const validateName = (name) => name.length > 2;

  const [name, setName] = useState(props.name);
  const [age, setAge] = useState(props.age);

  const [nameValid, setNameValid] = useState(validateName(initialName));
  const [ageValid, setAgeValid] = useState(validateName(initialAge));

  const onAgeChange = (e) => {
    setAge(e.target.value);
    setAgeValid(validateAge(e.target.value));
  };

  const onNameChange = (e) => {
    setName(e.target.value);
    setNameValid(validateName(e.target.value));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (nameValid && ageValid) {
      console.log(`Name: ${name} Age: ${age}`);
    } else console.log("Data is incorrect");
  };

  return (
    <form onSubmit={handleSubmit}>
      <p>
        <label>Name:</label>
        <br />
        <input
          type="text"
          value={name}
          onChange={onNameChange}
          style={{ borderColor: nameValid ? "green" : "red" }}
        ></input>
      </p>
      <p>
        <label>Age:</label>
        <br />
        <input
          type="number"
          value={age}
          onChange={onAgeChange}
          style={{ borderColor: ageValid ? "green" : "red" }}
        ></input>
      </p>
    </form>
  );
}

createRoot(document.getElementById("userForm")).render(<UserForm></UserForm>);
