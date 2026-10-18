import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { Button } from "./button";
import { buttonBase, buttonVariants } from "./button.styles";

const meta = {
  title: "UI/CalculatorButton",
  component: Button,
  args: { onClick: () => {} },
  decorators: [
    (Story) => (
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(4, 80px)",
          gridTemplateRows: "repeat(5, 48px)",
          gap: "10px",
        }}
      >
        <Story />
      </div>
    ),
  ],
} satisfies Meta<typeof Button>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Plain: Story = {
  args: { label: "7", row: 2, column: 1 },
};

export const Operator: Story = {
  args: { label: "+", row: 5, column: 4, className: buttonVariants.operator },
};

export const Ghost: Story = {
  args: { label: "CE", row: 1, column: 4, className: buttonVariants.ghost },
};

export const Disabled: Story = {
  args: { label: "0", row: 5, column: 1, disabled: true },
};

export const BaseOnly: Story = {
  args: { label: "5", row: 3, column: 2, className: buttonBase },
};
