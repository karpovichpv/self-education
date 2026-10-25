import { observer } from "mobx-react-lite";
import { CSSProperties } from "react";
import { ViewModel } from "../view-models/calculator-view-model";
import { Button } from "../components/ui/button/Button";
import { ButtonType } from "../components/ui/button/Button";

interface CalculationViewProps {
  viewModel: ViewModel;
}

interface KeypadButton {
  label: string;
  buttonType: ButtonType;
  column: number;
  row: number;
  onPress: (viewModel: ViewModel) => void;
}

const append = (value: string) => (viewModel: ViewModel) =>
  viewModel.appendChar(value);

const clear = (viewModel: ViewModel) => viewModel.pressClear();

const removeLast = (viewModel: ViewModel) => viewModel.pressBackspace();

const keypadStyle: CSSProperties = {
  display: "grid",
  gridTemplateColumns: "repeat(4, 1fr)",
  gridTemplateRows: "repeat(5, 1fr)",
  gap: "10px",
};

const keys: KeypadButton[] = [
  {
    label: "(",
    buttonType: ButtonType.Bracket,
    column: 1,
    row: 1,
    onPress: append("("),
  },
  {
    label: ")",
    buttonType: ButtonType.Bracket,
    column: 2,
    row: 1,
    onPress: append(")"),
  },
  {
    label: "CE",
    buttonType: ButtonType.Action,
    column: 3,
    row: 1,
    onPress: clear,
  },
  {
    label: "⌫",
    buttonType: ButtonType.Action,
    column: 4,
    row: 1,
    onPress: removeLast,
  },
  {
    label: "7",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 2,
    onPress: append("7"),
  },
  {
    label: "8",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 2,
    onPress: append("8"),
  },
  {
    label: "9",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 2,
    onPress: append("9"),
  },
  {
    label: "/",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 2,
    onPress: append("/"),
  },

  {
    label: "4",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 3,
    onPress: append("4"),
  },
  {
    label: "5",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 3,
    onPress: append("5"),
  },
  {
    label: "6",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 3,
    onPress: append("6"),
  },
  {
    label: "*",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 3,
    onPress: append("*"),
  },

  {
    label: "1",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 4,
    onPress: append("1"),
  },
  {
    label: "2",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 4,
    onPress: append("2"),
  },
  {
    label: "3",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 4,
    onPress: append("3"),
  },
  {
    label: "-",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 4,
    onPress: append("-"),
  },

  {
    label: "0",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 5,
    onPress: append("0"),
  },
  {
    label: ".",
    buttonType: ButtonType.Operator,
    column: 2,
    row: 5,
    onPress: append("."),
  },
  {
    label: "+",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 5,
    onPress: append("+"),
  },
];

export const CalculatorView = observer(function CalculatorView({
  viewModel,
}: CalculationViewProps) {
  return (
    <div>
      <p className="text-3xl font-bold">Expression: {viewModel.expression}</p>
      <output className="text-3xl font-bold" aria-live="polite">
        Result: {viewModel.result}
      </output>
      <div style={keypadStyle}>
        {keys.map((key) => (
          <Button
            key={key.label}
            label={key.label}
            buttonType={key.buttonType}
            column={key.column}
            row={key.row}
            onClick={() => key.onPress(viewModel)}
          />
        ))}
      </div>
    </div>
  );
});
