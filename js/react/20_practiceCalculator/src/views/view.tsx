import React from "react";
import { observer } from "mobx-react-lite";
import { ViewModel } from "../viewmodels/viewmodel";
import "../index.css";

interface CalculationViewProps {
  viewModel: ViewModel;
}

// Обертка observer() автоматически перерисует интерфейс при изменении viewModel.count
export const CalculatorView: React.FC<CalculationViewProps> = observer(
  ({ viewModel }) => {
    return (
      <>
        <div>
          <h1>Счетчик: {viewModel.getCalculatedString()}</h1>
          <div className="grid-container">
            <button className="grid-btn" onClick={() => viewModel.add1()}>
              1
            </button>
            <button onClick={() => viewModel.add2()}>2</button>
            <button onClick={() => viewModel.add3()}>3</button>
            <button onClick={() => viewModel.add4()}>4</button>
            <button onClick={() => viewModel.add5()}>5</button>
            <button onClick={() => viewModel.add6()}>6</button>
            <button onClick={() => viewModel.add7()}>7</button>
            <button onClick={() => viewModel.add8()}>8</button>
            <button onClick={() => viewModel.add9()}>9</button>
            <button onClick={() => viewModel.add0()}>0</button>
            <button onClick={() => viewModel.addPlus()}>+</button>
            <button onClick={() => viewModel.addMinus()}>-</button>
            <button onClick={() => viewModel.addMultiply()}>*</button>
            <button onClick={() => viewModel.addDevide()}>/</button>
            <button onClick={() => viewModel.addLeftBracket()}>(</button>
            <button onClick={() => viewModel.addRightBracket()}>)</button>
          </div>
        </div>
      </>
    );
  },
);
