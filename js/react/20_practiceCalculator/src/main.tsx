import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { CalculatorViewModel } from "./view-models/calculator-view-model";
import "./index.css";

const viewModel = new CalculatorViewModel();
const element = document.getElementById("root");

if (element != null) {
  createRoot(element).render(
    <StrictMode>
      <App viewModel={viewModel} />
    </StrictMode>,
  );
}
