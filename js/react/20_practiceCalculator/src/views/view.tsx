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
          <h1 className="text-3xl font-bold">
            Счетчик: {viewModel.getCalculatedString()}
          </h1>
          <h1 className="text-3xl font-bold">
            Счетчик: {viewModel.calculate()}
          </h1>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(4, 1fr)",
              gridTemplateRows: "repeat(5, 1fr)",
              gap: "10px",
            }}
          >
            <Button
              label="1"
              onClick={() => viewModel.add1()}
              column={1}
              row={4}
            />
            <Button
              label="2"
              onClick={() => viewModel.add2()}
              column={2}
              row={4}
            />
            <Button
              label="3"
              onClick={() => viewModel.add3()}
              column={3}
              row={4}
            />
            <Button
              label="4"
              onClick={() => viewModel.add4()}
              column={1}
              row={3}
            />
            <Button
              label="5"
              onClick={() => viewModel.add5()}
              column={2}
              row={3}
            />
            <Button
              label="6"
              onClick={() => viewModel.add6()}
              column={3}
              row={3}
            />
            <Button
              label="7"
              onClick={() => viewModel.add7()}
              column={1}
              row={2}
            />
            <Button
              label="8"
              onClick={() => viewModel.add8()}
              column={2}
              row={2}
            />
            <Button
              label="9"
              onClick={() => viewModel.add9()}
              column={3}
              row={2}
            />
            <Button
              label="0"
              onClick={() => viewModel.add0()}
              column={1}
              row={5}
            />
            <Button
              label="("
              onClick={() => viewModel.addRightBracket()}
              column={1}
              row={1}
            />
            <Button
              label=")"
              onClick={() => viewModel.addLeftBracket()}
              column={2}
              row={1}
            />
            <Button
              label="/"
              onClick={() => viewModel.addDevide()}
              column={4}
              row={2}
            />
            <Button
              label="*"
              onClick={() => viewModel.addMultiply()}
              column={4}
              row={3}
            />
            <Button
              label="-"
              onClick={() => viewModel.addMinus()}
              column={4}
              row={4}
            />
            <Button
              label="+"
              onClick={() => viewModel.addPlus()}
              column={4}
              row={5}
            />
            <Button
              label="="
              onClick={() => viewModel.addEqualSign()}
              column={3}
              row={5}
            />
            <Button
              className="bg-amber-500 hover:bg-amber-600"
              label="."
              onClick={() => viewModel.addEqualDot()}
              column={2}
              row={5}
            />
            <Button
              label="%"
              onClick={() => viewModel.addPercent()}
              column={3}
              row={1}
            />
            <Button
              label="CE"
              onClick={() => viewModel.addCe()}
              column={4}
              row={1}
            />
          </div>
        </div>
      </>
    );
  },
);
