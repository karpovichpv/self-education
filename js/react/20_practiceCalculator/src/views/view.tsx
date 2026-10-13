import React from "react";
import { observer } from "mobx-react-lite";
import { ViewModel } from "../viewmodels/viewmodel";

interface CalculationViewProps {
  viewModel: ViewModel;
}

// Обертка observer() автоматически перерисует интерфейс при изменении viewModel.count
export const CalculatorView: React.FC<CalculationViewProps> = observer(
  ({ viewModel }) => {
    return (
      <div >
        <h1>Счетчик: {viewModel.getNumber()}</h1>
        <button onClick ={()=> viewModel.increment()}>Increment</button>
      </div>
    );
  },
);
