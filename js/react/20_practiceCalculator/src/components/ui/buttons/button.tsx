import React from "react";

interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  row?: number;
  column?: number;
}

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  disabled = false,
  row: gridRow = 1,
  column: gridColumn = 1,
}) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      style={{
        padding: "8px 16px",
        cursor: "pointer",
        backgroundColor: "red",
        gridColumnStart: gridColumn,
        gridRowStart: gridRow,
      }}
    >
      {label}
    </button>
  );
};
