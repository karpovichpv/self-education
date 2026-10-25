import { ViewModel } from "./Viewmodels/Viewmodel";
import { CalculatorView } from "./Views/View";
import { createRoot } from "react-dom/client";
import "./index.css";

const viewModel = new ViewModel();

export default function App() {
  return <CalculatorView viewModel={viewModel} />;
}

const element = document.getElementById("app");
if (element != null) {
  createRoot(element).render(<App></App>);
}
