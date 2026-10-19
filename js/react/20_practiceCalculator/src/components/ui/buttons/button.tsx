import React from "react";
import { buttonNumber, buttonOperator } from "./button.styles";

export enum ButtonType {
  Number,
  Operator,
  Manage,
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
  let appliedClassName = buttonNumber;
  if (buttonType === ButtonType.Manage) appliedClassName = buttonOperator;

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
