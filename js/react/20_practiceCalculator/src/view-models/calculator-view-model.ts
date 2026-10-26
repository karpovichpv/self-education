import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

const ERROR_TEXT = "Error";

export class CalculatorViewModel {
  evaluator: CalculationModel = new CalculationModel();
  expression: string = "";

  constructor() {
    makeAutoObservable(this, { evaluator: false });
  }

  get result(): string {
    const evaluation = this.evaluator.evaluate(this.expression);
    if (evaluation.ok) {
      return evaluation.display;
    }
    if (evaluation.reason === "empty" || evaluation.reason === "incomplete") {
      return "";
    }
    return ERROR_TEXT;
  }

  appendChar = (input: string): void => {
    this.expression += input;
  };

  pressBackspace = (): void => {
    this.expression = this.expression.slice(0, -1);
  };

  pressClear = (): void => {
    this.expression = "";
  };
}
