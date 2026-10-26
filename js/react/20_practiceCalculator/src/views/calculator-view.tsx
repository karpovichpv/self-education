import { observer } from "mobx-react-lite";
import { CSSProperties } from "react";
import { CalculatorViewModel } from "../view-models/calculator-view-model";
import { Button } from "../Components/Ui/Button/Button";
import { ButtonType } from "../Components/Ui/Button/Button";

interface CalculatorViewProps {
  viewModel: CalculatorViewModel;
}

interface KeypadButton {
  label: string;
  buttonType: ButtonType;
  column: number;
  row: number;
  onPress: (viewModel: CalculatorViewModel) => void;
}

const pressAppend = (value: string) => (viewModel: CalculatorViewModel) =>
  viewModel.appendChar(value);

const pressClear = (viewModel: CalculatorViewModel) => viewModel.pressClear();

const pressBackspace = (viewModel: CalculatorViewModel) =>
  viewModel.pressBackspace();

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
    onPress: pressAppend("("),
  },
  {
    label: ")",
    buttonType: ButtonType.Bracket,
    column: 2,
    row: 1,
    onPress: pressAppend(")"),
  },
  {
    label: "CE",
    buttonType: ButtonType.Action,
    column: 3,
    row: 1,
    onPress: pressClear,
  },
  {
    label: "⌫",
    buttonType: ButtonType.Action,
    column: 4,
    row: 1,
    onPress: pressBackspace,
  },
  {
    label: "7",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 2,
    onPress: pressAppend("7"),
  },
  {
    label: "8",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 2,
    onPress: pressAppend("8"),
  },
  {
    label: "9",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 2,
    onPress: pressAppend("9"),
  },
  {
    label: "/",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 2,
    onPress: pressAppend("/"),
  },

  {
    label: "4",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 3,
    onPress: pressAppend("4"),
  },
  {
    label: "5",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 3,
    onPress: pressAppend("5"),
  },
  {
    label: "6",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 3,
    onPress: pressAppend("6"),
  },
  {
    label: "*",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 3,
    onPress: pressAppend("*"),
  },

  {
    label: "1",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 4,
    onPress: pressAppend("1"),
  },
  {
    label: "2",
    buttonType: ButtonType.Digit,
    column: 2,
    row: 4,
    onPress: pressAppend("2"),
  },
  {
    label: "3",
    buttonType: ButtonType.Digit,
    column: 3,
    row: 4,
    onPress: pressAppend("3"),
  },
  {
    label: "-",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 4,
    onPress: pressAppend("-"),
  },

  {
    label: "0",
    buttonType: ButtonType.Digit,
    column: 1,
    row: 5,
    onPress: pressAppend("0"),
  },
  {
    label: ".",
    buttonType: ButtonType.Operator,
    column: 2,
    row: 5,
    onPress: pressAppend("."),
  },
  {
    label: "+",
    buttonType: ButtonType.Operator,
    column: 4,
    row: 5,
    onPress: pressAppend("+"),
  },
];

export const CalculatorView = observer(function CalculatorView({
  viewModel,
}: CalculatorViewProps) {
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
