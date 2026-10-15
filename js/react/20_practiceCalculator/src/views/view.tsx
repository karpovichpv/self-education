import React from "react";
import { observer } from "mobx-react-lite";
import { ViewModel } from "../viewmodels/viewmodel";
import { Button } from "../components/ui/buttons/button";

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
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(3, 1fr)",
              gridTemplateRows: "repeat(3, 1fr)",
              gap: "10px",
            }}
          >
            <Button
              label="1"
              onClick={() => viewModel.add1()}
              gridColumn="2"
              gridRow="2"
            />
            <Button
              label="2"
              onClick={() => viewModel.add2()}
              gridColumn="1"
              gridRow="1"
            />
            {/* <Button label="3" onClick={() => viewModel.add3()} />
            <Button label="4" onClick={() => viewModel.add4()} />
            <Button label="5" onClick={() => viewModel.add5()} />
            <Button label="6" onClick={() => viewModel.add6()} />
            <Button label="7" onClick={() => viewModel.add7()} />
            <Button label="8" onClick={() => viewModel.add8()} />
            <Button label="9" onClick={() => viewModel.add9()} />
            <Button label="0" onClick={() => viewModel.add0()} />
            <Button label="+" onClick={() => viewModel.addPlus()} />
            <Button label="-" onClick={() => viewModel.addMinus()} />
            <Button label="*" onClick={() => viewModel.addMultiply()} />
            <Button label="/" onClick={() => viewModel.addDevide()} />
            <Button label="(" onClick={() => viewModel.addRightBracket()} />
            <Button label=")" onClick={() => viewModel.addLeftBracket()} />
            */}
          </div>
        </div>
      </>
    );
  },
);
