const buttonBase = [
  "h-full",
  "w-full",
  "cursor-pointer",
  "rounded-md",
  "px-4",
  "py-2",
  "text-lg",
  "text-black",
  "transition-colors",
  "duration-300",
  "col-start-(--col)",
  "row-start-(--row)",
  "disabled:cursor-not-allowed",
  "disabled:opacity-40",
];

const withColor = (color: string) => [...buttonBase, color].join(" ");

export const buttonNumber = withColor("bg-red-500");
export const buttonOperator = withColor("bg-purple-500");
export const buttonManage = withColor("bg-green-500");
export const buttonBracket = withColor("bg-blue-500");
export const buttonUnset = withColor("bg-cyan-500");
