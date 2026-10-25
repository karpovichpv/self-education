import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { ViewModel } from "./view-models/calculator-view-model";
import "./index.css";

const viewModel = new ViewModel();
const element = document.getElementById("root");

if (element != null) {
  createRoot(element).render(
    <StrictMode>
      <App viewModel={viewModel} />
    </StrictMode>,
  );
}
