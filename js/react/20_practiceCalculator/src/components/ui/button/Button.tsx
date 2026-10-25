import { CSSProperties } from "react";
import {
  buttonBracket,
  buttonManage,
  buttonNumber,
  buttonOperator,
} from "./Button.styles";

export enum ButtonType {
  Digit,
  Operator,
  Action,
  Bracket,
}

interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  row?: number;
  column?: number;
  className?: string;
  buttonType: ButtonType;
}

const buttonTypeStyles: Record<ButtonType, string> = {
  [ButtonType.Digit]: buttonNumber,
  [ButtonType.Operator]: buttonOperator,
  [ButtonType.Action]: buttonManage,
  [ButtonType.Bracket]: buttonBracket,
};

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  buttonType,
  disabled = false,
  row = 1,
  column = 1,
  className = "",
}: ButtonProps) => {
  const gridPosition = { "--col": column, "--row": row } as CSSProperties;

  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className={[buttonTypeStyles[buttonType], className]
        .filter(Boolean)
        .join(" ")}
      style={gridPosition}
    >
      {label}
    </button>
  );
};
