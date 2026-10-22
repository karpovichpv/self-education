import { evaluate } from "mathjs";

export class CalculationModel {
  evaluate(input: string): string {
    if (input.trim() === "") {
      return "";
    }
    try {
      const value: unknown = evaluate(input);
      if (typeof value !== "number" || !Number.isFinite(value)) {
        return "Ошибка";
      }

      return String(Number(value.toPrecision(12)));
    } catch {
      return "Ошибка";
    }
  }
}
