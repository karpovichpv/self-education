import type { Meta, StoryObj } from "@storybook/react-vite";
import { Button, ButtonType } from "./Button";

const meta = {
  title: "UI/CalculatorButton",
  component: Button,
  args: { label: "5", row: 3, column: 2, onClick: () => {} },
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

const NumberKey: Story = {
  args: { label: "7", buttonType: ButtonType.Number, row: 2, column: 1 },
};

const OperatorKey: Story = {
  args: { label: "+", buttonType: ButtonType.Operator, row: 5, column: 4 },
};

const ManageKey: Story = {
  args: { label: "CE", buttonType: ButtonType.Manage, row: 1, column: 3 },
};

const BracketKey: Story = {
  args: { label: "(", buttonType: ButtonType.Bracket, row: 1, column: 4 },
};

// `Number` is a JS global, so the local names avoid shadowing it inside this module.
export {
  NumberKey as Number,
  OperatorKey as Operator,
  ManageKey as Manage,
  BracketKey as Bracket,
};

export const Disabled: Story = {
  args: {
    label: "0",
    buttonType: ButtonType.Number,
    row: 5,
    column: 1,
    disabled: true,
  },
};

export const CustomClassName: Story = {
  args: {
    label: "9",
    buttonType: ButtonType.Number,
    row: 2,
    column: 3,
    className: "text-white shadow-md",
  },
};

export const AllTypes: Story = {
  render: (args) => (
    <>
      <Button
        {...args}
        label="8"
        buttonType={ButtonType.Number}
        row={2}
        column={2}
      />
      <Button
        {...args}
        label="*"
        buttonType={ButtonType.Operator}
        row={3}
        column={4}
      />
      <Button
        {...args}
        label="remove"
        buttonType={ButtonType.Manage}
        row={1}
        column={1}
      />
      <Button
        {...args}
        label=")"
        buttonType={ButtonType.Bracket}
        row={1}
        column={2}
      />
    </>
  ),
};
