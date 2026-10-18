import React from "react";
import { buttonBase } from "./button.styles";

interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  row?: number;
  column?: number;
  className?: string;
}

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  disabled = false,
  row: gridRow = 1,
  column: gridColumn = 1,
  className = "",
}) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={[buttonBase, className].filter(Boolean).join(" ")}
      style={{ "--col": gridColumn, "--row": gridRow } as React.CSSProperties}
    >
      {label}
    </button>
  );
};
