export const buttonBase = [
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

export const buttonNumber = [...buttonBase, "bg-red-500"].join(" ");
export const buttonOperator = [...buttonBase, "bg-purple-500"].join(" ");
export const buttonManage = [...buttonBase, "bg-green-500"].join(" ");
export const buttonBracket = [...buttonBase, "bg-blue-500"].join(" ");

export const buttonVariants = {
  operator: "bg-amber-500 hover:bg-amber-600",
  ghost: "bg-transparent text-slate-300",
};
