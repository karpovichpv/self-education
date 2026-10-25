import { ViewModel } from "./view-models/calculator-view-model";
import { CalculatorView } from "./views/calculator-view";

interface AddProps {
  viewModel: ViewModel;
}

export default function App({ viewModel }: AddProps) {
  return <CalculatorView viewModel={viewModel} />;
}
