import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../Models/CalculationModel";

const ERROR_TEXT = "Error";

export class ViewModel {
  calculatedModel: CalculationModel = new CalculationModel();
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  get result(): string {
    const evaluation = this.calculatedModel.evaluate(this.calculatedString);
    return evaluation.ok ? evaluation.display : ERROR_TEXT;
  }

  addChar = (input: string): void => {
    this.calculatedString += input;
  };

  removeLast = (): void => {
    this.calculatedString = this.calculatedString.slice(0, -1);
  };

  clear = (): void => {
    this.calculatedString = "";
  };

  equals = (): void => {
    const evaluation = this.calculatedModel.evaluate(this.calculatedString);
    if (evaluation.ok) {
      this.calculatedString = evaluation.display;
    }
  };
}
