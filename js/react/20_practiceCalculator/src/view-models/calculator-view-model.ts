import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

const ERROR_TEXT = "Error";

export class ViewModel {
  evaluator: CalculationModel = new CalculationModel();
  expression: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  get result(): string {
    const evaluation = this.evaluator.evaluate(this.expression);
    if (evaluation.ok) {
      return evaluation.display;
    }
    return evaluation.reason === "empty" ? "" : ERROR_TEXT;
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
