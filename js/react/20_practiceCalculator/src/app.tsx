import React from "react";
import { ViewModel } from "./viewmodels/viewmodel";
import { CalculatorView } from "./views/view";
import { createRoot } from "react-dom/client";

// Инициализируем ViewModel
const viewModel = new ViewModel();

export default function App() {
  return <CalculatorView viewModel={viewModel} />;
}

const element = document.getElementById("app");
if (element != null) {
  createRoot(element).render(<App></App>);
}
