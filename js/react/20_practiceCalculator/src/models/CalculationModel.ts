import { evaluate as mathEvaluate } from "mathjs";

const INCOMPLETE_TAIL = /[+\-*/.(]\s*$/;

export type EvaluationResult =
  | { ok: true; display: string }
  | { ok: false; reason: "empty" | "incomplete" | "invalid" | "not-a-number" };

export class CalculationModel {
  evaluate(input: string): EvaluationResult {
    if (input.trim() === "") {
      return { ok: false, reason: "empty" };
    }
    try {
      const value: unknown = mathEvaluate(input);
      if (typeof value !== "number" || !Number.isFinite(value)) {
        return { ok: false, reason: "not-a-number" };
      }
      return { ok: true, display: String(Number(value.toPrecision(12))) };
    } catch {
      return INCOMPLETE_TAIL.test(input)
        ? { ok: false, reason: "incomplete" }
        : { ok: false, reason: "invalid" };
    }
  }
}
