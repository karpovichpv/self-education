import React from "react";
import { observer } from "mobx-react-lite";
import { ViewModel } from "../viewmodels/viewmodel";
import { Button } from "../components/ui/buttons/button";
import { ButtonType } from "../components/ui/buttons/button";
import { column } from "mathjs";

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
            Выражение: {viewModel.calculatedString}
          </h1>
          <h1 className="text-3xl font-bold">Результат: {viewModel.result}</h1>
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
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("1")}
              column={1}
              row={4}
            />
            <Button
              label="2"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("2")}
              column={2}
              row={4}
            />
            <Button
              label="3"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("3")}
              column={3}
              row={4}
            />
            <Button
              label="4"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("4")}
              column={1}
              row={3}
            />
            <Button
              label="5"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("5")}
              column={2}
              row={3}
            />
            <Button
              label="6"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("6")}
              column={3}
              row={3}
            />
            <Button
              label="7"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("7")}
              column={1}
              row={2}
            />
            <Button
              label="8"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("8")}
              column={2}
              row={2}
            />
            <Button
              label="9"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("9")}
              column={3}
              row={2}
            />
            <Button
              label="0"
              buttonType={ButtonType.Number}
              onClick={() => viewModel.addChar("0")}
              column={1}
              row={5}
            />
            <Button
              label=")"
              buttonType={ButtonType.Bracket}
              onClick={() => viewModel.addChar(")")}
              column={2}
              row={1}
            />
            <Button
              label="("
              buttonType={ButtonType.Bracket}
              onClick={() => viewModel.addChar("(")}
              column={1}
              row={1}
            />
            <Button
              label="/"
              buttonType={ButtonType.Operator}
              onClick={() => viewModel.addChar("/")}
              column={4}
              row={2}
            />
            <Button
              label="*"
              buttonType={ButtonType.Operator}
              onClick={() => viewModel.addChar("*")}
              column={4}
              row={3}
            />
            <Button
              label="-"
              buttonType={ButtonType.Operator}
              onClick={() => viewModel.addChar("-")}
              column={4}
              row={4}
            />
            <Button
              label="+"
              buttonType={ButtonType.Operator}
              onClick={() => viewModel.addChar("+")}
              column={4}
              row={5}
            />
            <Button
              buttonType={ButtonType.Operator}
              label="."
              onClick={() => viewModel.addChar(".")}
              column={2}
              row={5}
            />
            <Button
              label="remove"
              className=""
              buttonType={ButtonType.Manage}
              onClick={() => viewModel.removeLast()}
              column={4}
              row={1}
            />
            <Button
              label="CE"
              buttonType={ButtonType.Manage}
              onClick={() => viewModel.clear()}
              column={3}
              row={1}
            />
          </div>
        </div>
      </>
    );
  },
);
