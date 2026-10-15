import React from "react";

interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  gridRow?: string;
  gridColumn?: string;
}

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  disabled = false,
  gridRow = "0",
  gridColumn = "0",
}) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      style={{
        padding: "8px 16px",
        cursor: "pointer",
        backgroundColor: "red",
        gridColumn: gridColumn,
        gridRow: gridRow,
      }}
    >
      {label}
    </button>
  );
};
