import React from "react";
import {
  buttonNumber as buttonNumberStyle,
  buttonOperator as buttonOperatorStyle,
  buttonManage as buttonManageStyle,
  buttonBracket as buttonBracketStyle,
} from "./button.styles";

export enum ButtonType {
  Number,
  Operator,
  Manage,
  Bracket,
}

interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  row?: number;
  column?: number;
  className?: string;
  buttonType?: ButtonType;
}

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  disabled = false,
  row: gridRow = 1,
  column: gridColumn = 1,
  className = "",
  buttonType = ButtonType.Manage,
}) => {
  let appliedClassName;
  if (buttonType === ButtonType.Manage) appliedClassName = buttonManageStyle;
  if (buttonType === ButtonType.Number) appliedClassName = buttonNumberStyle;
  if (buttonType === ButtonType.Operator)
    appliedClassName = buttonOperatorStyle;
  if (buttonType === ButtonType.Bracket) appliedClassName = buttonBracketStyle;

  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={[appliedClassName, className].filter(Boolean).join(" ")}
      style={{ "--col": gridColumn, "--row": gridRow } as React.CSSProperties}
    >
      {label}
    </button>
  );
};
