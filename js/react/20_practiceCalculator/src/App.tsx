import { CalculatorViewModel } from "./view-models/calculator-view-model";
import { CalculatorView } from "./views/calculator-view";

interface AppProps {
  viewModel: CalculatorViewModel;
}

export default function App({ viewModel }: AppProps) {
  return <CalculatorView viewModel={viewModel} />;
}
